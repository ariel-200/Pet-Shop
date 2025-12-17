namespace PetShop.Models
{
    // Represents a pet category such as Dog, Cat, Bird, etc.
    public class Category
    {
        // Primary key for the Category table
        public int CategoryId { get; set; }

        // The name of the category
        public string Name { get; set; } = string.Empty;

        // A category can contain many animals.
        public List<Animal> Animals { get; set; } = new();
    }
}
