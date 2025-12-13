using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.Models;

namespace PetShop.Pages.Admin
{
    // Admin page for creating a new animal
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public Animal Animal { get; set; } = new();

        // Dropdown options for categories/species
        public List<SelectListItem> CategoryOptions { get; set; } = new();

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        // GET: show form + load dropdown
        public async Task OnGetAsync()
        {
            await LoadCategoriesAsync();
        }

        // POST: save new animal
        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCategoriesAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Animals.Add(Animal);
            await _context.SaveChangesAsync();

            return RedirectToPage("Admin");
        }

        // Loads category list from database for dropdown
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
