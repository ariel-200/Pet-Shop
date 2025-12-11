using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.Models;

namespace PetShop.Pages.Animals
{
    // Code-behind for the Animals listing page with filter support
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        // Holds the list of animals to display
        public List<Animal> Animals { get; set; } = new();

        // Holds the list of categories (species) for the filter dropdown
        public List<Category> Categories { get; set; } = new();

        // Bound property for the selected CategoryId.
        // SupportsGet = true allows this value to be read from the query string (?SelectedCategoryId=1).
        [BindProperty(SupportsGet = true)]
        public int? SelectedCategoryId { get; set; }

        // Inject the database context
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        // Handles GET requests to /Animals
        public async Task OnGetAsync()
        {
            // Load all categories from the database for the filter dropdown
            Categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            // Start with a base query of available animals
            var query = _context.Animals
                .Include(a => a.Category)
                .Where(a => a.IsAvailable);

            // If the user selected a species (category), filter the animals
            if (SelectedCategoryId.HasValue)
            {
                query = query.Where(a => a.CategoryId == SelectedCategoryId.Value);
            }

            // Execute the query and store the results in the Animals list
            Animals = await query.ToListAsync();
        }
    }
}
