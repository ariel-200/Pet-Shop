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

        // All animals currently in the user's cart
        public List<CartItem> CartItems { get; set; } = new();

        // Calculated totals
        public decimal Subtotal { get; set; }
        public decimal SalesTax { get; set; }
        public decimal FinalTotal { get; set; }

        // Sales tax rate (7%)
        private const decimal TaxRate = 0.07m;

        // Inject the database context
        public ShoppingCartModel(AppDbContext context)
        {
            _context = context;
        }

        // Handles GET /ShoppingCart
        // Loads the cart from session and calculates totals.
        public void OnGet()
        {
            LoadCartAndTotals();
        }

        // Handles POST /ShoppingCart?handler=Remove
        // Removes a single animal from the cart and marks it available again.
        public async Task<IActionResult> OnPostRemoveAsync(int animalId)
        {
            // Load the cart from session
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            // Find the matching cart item
            var itemToRemove = cart.FirstOrDefault(c => c.AnimalId == animalId);
            if (itemToRemove != null)
            {
                // Remove it from the in-memory cart list
                cart.Remove(itemToRemove);

                // Save the updated cart back into session
                HttpContext.Session.SetObject("Cart", cart);

                // Mark the animal as available again in the database
                var animal = await _context.Animals.FindAsync(animalId);
                if (animal != null)
                {
                    animal.IsAvailable = true;
                    await _context.SaveChangesAsync();
                }
            }

            // Redirect to GET so the page refreshes with updated data
            return RedirectToPage();
        }

        // Handles POST /ShoppingCart?handler=Clear
        // Clears the entire cart and marks all animals as available.
        public async Task<IActionResult> OnPostClearAsync()
        {
            // Load the cart from session
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            // For each cart item, mark its animal as available again
            foreach (var item in cart)
            {
                var animal = await _context.Animals.FindAsync(item.AnimalId);
                if (animal != null)
                {
                    animal.IsAvailable = true;
                }
            }

            // Save all availability changes in one database call
            await _context.SaveChangesAsync();

            // Remove the cart from session
            HttpContext.Session.Remove("Cart");

            // Redirect to GET to show an empty cart
            return RedirectToPage();
        }

        // Helper method that loads the cart from session and calculates the totals
        private void LoadCartAndTotals()
        {
            // Load the cart; if nothing is stored, use an empty list
            CartItems = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                        ?? new List<CartItem>();

            // Subtotal is the sum of all animal prices
            Subtotal = CartItems.Sum(item => item.Price);

            // Calculate tax and final total
            SalesTax = Subtotal * TaxRate;
            FinalTotal = Subtotal + SalesTax;
        }
    }
}
