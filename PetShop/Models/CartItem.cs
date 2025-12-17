namespace PetShop.Models
{
    // Represents a single animal in the shopping cart.
    
    public class CartItem
    {
        public int AnimalId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Species { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}
