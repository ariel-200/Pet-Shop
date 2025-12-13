using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.Models;

namespace PetShop.Pages.Admin
{
    // Admin page for editing an existing animal record
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        // Bind the form fields directly to this Animal object
        [BindProperty]
        public Animal Animal { get; set; } = new();

        // Dropdown list for Category/Species options
        public List<SelectListItem> CategoryOptions { get; set; } = new();

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Edit/5
        // Loads the animal and the species dropdown
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Load dropdown options first
            await LoadCategoriesAsync();

            // Find the animal by primary key
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            // Copy DB record into the bound property so the form is pre-filled
            Animal = animal;

            return Page();
        }

        // POST: /Admin/Edit/5
        // Saves the updated animal back to the database
        public async Task<IActionResult> OnPostAsync()
        {
            // Reload dropdown options (needed if validation fails and page reloads)
            await LoadCategoriesAsync();

            // If validation fails, re-display the page with errors
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Tell EF Core this record should be updated
            _context.Attach(Animal).State = EntityState.Modified;

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return to the main admin dashboard
            return RedirectToPage("Admin");
        }

        // Helper method to load Category dropdown data from the database
        private async Task LoadCategoriesAsync()
        {
            CategoryOptions = await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                })
                .ToListAsync();
        }
    }
}
