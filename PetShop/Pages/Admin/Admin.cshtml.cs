using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.Models;

namespace PetShop.Pages.Admin
{
    // Main Admin dashboard page for managing animals
    public class AdminModel : PageModel
    {
        private readonly AppDbContext _context;

        // List of animals displayed in the admin table
        public List<Animal> Animals { get; set; } = new();

        public AdminModel(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Admin
        public async Task OnGetAsync()
        {
            // Load all animals + category for species name
            Animals = await _context.Animals
                .Include(a => a.Category)
                .OrderBy(a => a.Name)
                .ToListAsync();
        }

        // POST: /Admin/Admin?handler=ToggleAvailability
        // Quick toggle for IsAvailable (true/false)
        public async Task<IActionResult> OnPostToggleAvailabilityAsync(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            // Flip availability
            animal.IsAvailable = !animal.IsAvailable;
            await _context.SaveChangesAsync();

            // Reload admin dashboard
            return RedirectToPage("Admin");
        }
    }
}
