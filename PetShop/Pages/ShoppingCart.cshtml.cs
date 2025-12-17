using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.Helpers;
using PetShop.Models;

namespace PetShop.Pages
{
    // PageModel for displaying and managing the shopping cart.
    public class ShoppingCartModel : PageModel
    {
        private readonly AppDbContext _context;

        // Items currently in the cart
        public List<CartItem> CartItems { get; set; } = new();

        // Totals
        public decimal Subtotal { get; set; }
        public decimal SalesTax { get; set; }
        public decimal FinalTotal { get; set; }

        private const decimal TaxRate = 0.07m;

        public ShoppingCartModel(AppDbContext context)
        {
            _context = context;
        }

        // GET ShoppingCart
        public void OnGet()
        {
            LoadCartAndTotals();
        }

        // Remove a single animal and make it available again
        public async Task<IActionResult> OnPostRemoveAsync(int animalId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            var itemToRemove = cart.FirstOrDefault(c => c.AnimalId == animalId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.SetObject("Cart", cart);

                // Make the animal available again
                var animal = await _context.Animals.FindAsync(animalId);
                if (animal != null)
                {
                    animal.IsAvailable = true;
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage();
        }

        // Clear cart and restore availability
        public async Task<IActionResult> OnPostClearAsync()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            foreach (var item in cart)
            {
                var animal = await _context.Animals.FindAsync(item.AnimalId);
                if (animal != null)
                {
                    animal.IsAvailable = true;
                }
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");

            return RedirectToPage();
        }

        // Checkout
        // Saves checkout summary, clears cart, pets remain unavailable
        public IActionResult OnPostCheckout()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            // Calculate totals
            var subtotal = cart.Sum(i => i.Price);
            var tax = subtotal * TaxRate;
            var finalTotal = subtotal + tax;

            // Save checkout summary to session
            HttpContext.Session.SetObject("CheckoutItems", cart);
            HttpContext.Session.SetObject("CheckoutTotal", finalTotal);

            // Clear the cart
            HttpContext.Session.Remove("Cart");

            return RedirectToPage("/Checkout");
        }

        // Helper to load cart and totals
        private void LoadCartAndTotals()
        {
            CartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                        ?? new List<CartItem>();

            Subtotal = CartItems.Sum(item => item.Price);
            SalesTax = Subtotal * TaxRate;
            FinalTotal = Subtotal + SalesTax;
        }
    }
}
