using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.Models;

namespace PetShop.Pages.Animals
{
    // Code-behind for the Animals listing page
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        // Inject the database context into the page model
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        // Holds the list of animals returned from the database
        public List<Animal> Animals { get; set; } = new();

        // Handles GET requests when visiting /Animals
        public async Task OnGetAsync(int? categoryId)
        {
            // Base query: only load animals marked as available
            var query = _context.Animals
                .Include(a => a.Category)      // Join Category table
                .Where(a => a.IsAvailable);     // Only available animals

            // Optional filter: if categoryId is provided, filter by species
            if (categoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == categoryId.Value);
            }

            // Execute the query asynchronously and load into Animals list
            Animals = await query.ToListAsync();
        }
    }
}
