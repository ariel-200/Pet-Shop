namespace PetShop.Models
{
    // Represents a single animal in the shopping cart.
    // Each animal can only appear once in the cart.
    public class CartItem
    {
        public int AnimalId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Species { get; set; } = string.Empty;

        // Price for this single animal
        public decimal Price { get; set; }
    }
}
