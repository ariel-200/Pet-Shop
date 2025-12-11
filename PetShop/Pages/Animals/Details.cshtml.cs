using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.Models;

namespace PetShop.Pages.Animals
{
    // Code-behind for the Animal Details page
    public class DetailsModel : PageModel
    {
        private readonly AppDbContext _context;

        // Property to hold the animal that will be displayed on the page
        public Animal? Animal { get; set; }

        // Inject the database context into the page model
        public DetailsModel(AppDbContext context)
        {
            _context = context;
        }

        // Handles GET requests like /Animals/Details?id=1
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            // If no id is provided in the query string, return 404
            if (id == null)
            {
                return NotFound();
            }

            // Look up the animal by its primary key and include the Category
            Animal = await _context.Animals
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.AnimalId == id.Value);

            // If the animal does not exist, return 404
            if (Animal == null)
            {
                return NotFound();
            }

            // If we got this far, the page will render with the Animal property filled
            return Page();
        }

        // We will add an OnPost handler later when we implement the shopping cart.
    }
}
