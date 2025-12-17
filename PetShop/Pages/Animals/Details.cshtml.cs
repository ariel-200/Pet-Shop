using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.Helpers;
using PetShop.Models;

namespace PetShop.Pages.Animals
{
    // Displays information about a single animal and handles adding it to the cart.
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        // The animal that will be displayed on the page
        public Animal? Animal { get; set; }

        // Inject the database context
        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        // Handles GET requests
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // If no id was supplied,
            if (id == null)
            {
                return NotFound();
            }

            // Look up the animal and include its Category for Species info
            Animal = await _context.Animals
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.AnimalId == id.Value);

            // If no animal is found with that ID
            if (Animal == null)
            {
                return NotFound();
            }

            return Page();
        }

        //  When the user clicks "Add to Cart"
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            // If the id is missing, send the user back to the list
            if (id == null)
            {
                return RedirectToPage("Index");
            }

            // Look up the animal again to make sure it still exists
            var animal = await _context.Animals
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.AnimalId == id.Value);

            if (animal == null)
            {
                return NotFound();
            }

            // If the animal is already marked as unavailable,
            // do not add it to the cart again.
            if (!animal.IsAvailable)
            {
                // Redirect to the cart or index
                return RedirectToPage("/ShoppingCart");
            }

            // Retrieve the current cart from session (or create a new empty list)
            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart")
                       ?? new List<CartItem>();

            // Check if this animal is already in the cart
            var existingItem = cart.FirstOrDefault(c => c.AnimalId == animal.AnimalId);

            if (existingItem == null)
            {
                // Add the animal to the cart if it is not already there
                cart.Add(new CartItem
                {
                    AnimalId = animal.AnimalId,
                    Name = animal.Name,
                    Species = animal.Category?.Name ?? string.Empty,
                    Price = animal.Price
                });

                // Mark the animal as unavailable so it no longer appears in the list
                animal.IsAvailable = false;

                // Save the IsAvailable change to the database
                await _context.SaveChangesAsync();

                // Save the updated cart back into session
                HttpContext.Session.SetObject("Cart", cart);
            }
            // If the animal is already in the cart, we don't add it again.

            // Redirect the user to the Shopping Cart page to see their reserved animals
            return RedirectToPage("/ShoppingCart");
        }
    }
}
