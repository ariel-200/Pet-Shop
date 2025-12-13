using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PetShop.Data;
using PetShop.Models;

namespace PetShop.Pages.Admin
{
    // Admin page for deleting an animal
    public class DeleteModel : PageModel
    {
        private readonly AppDbContext _context;

        public Animal? Animal { get; set; }

        public DeleteModel(AppDbContext context)
        {
            _context = context;
        }

        // GET: confirm delete
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Animal = await _context.Animals
                .Include(a => a.Category)
                .FirstOrDefaultAsync(a => a.AnimalId == id);

            if (Animal == null)
            {
                return NotFound();
            }

            return Page();
        }

        // POST: delete
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var animal = await _context.Animals.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return RedirectToPage("Admin");
        }
    }
}
