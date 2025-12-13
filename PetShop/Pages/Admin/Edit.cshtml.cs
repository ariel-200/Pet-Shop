using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.Models;

namespace PetShop.Pages.Admin
{
    // Admin page for editing an animal
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public Animal Animal { get; set; } = new();

        public List<SelectListItem> CategoryOptions { get; set; } = new();

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        // GET: load existing animal
        public async Task<IActionResult> OnGetAsync(int id)
        {
            await LoadCategoriesAsync();

            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            Animal = animal;
            return Page();
        }

        // POST: save changes
        public async Task<IActionResult> OnPostAsync()
        {
            await LoadCategoriesAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Animal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToPage("Admin");
        }

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
