using Microsoft.AspNetCore.Mvc.RazorPages;
using PetShop.Helpers;
using PetShop.Models;

namespace PetShop.Pages
{
    // PageModel for displaying checkout confirmation details
    public class CheckoutConfirmationModel : PageModel
    {
        // Pets that were checked out
        public List<CartItem> CheckedOutItems { get; set; } = new();

        // Final total price
        public decimal FinalTotal { get; set; }

        public void OnGet()
        {
            // Retrieve checkout summary from session
            CheckedOutItems = HttpContext.Session.GetObject<List<CartItem>>("CheckoutItems")
                               ?? new List<CartItem>();

            FinalTotal = HttpContext.Session.GetObject<decimal>("CheckoutTotal");

            // Clear checkout summary 
            HttpContext.Session.Remove("CheckoutItems");
            HttpContext.Session.Remove("CheckoutTotal");
        }
    }
}
