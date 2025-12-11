using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetShop.Models
{
    // Represents an adoptable or purchasable animal in the store
    public class Animal
    {
        // Primary key for the Animal table
        public int AnimalId { get; set; }

        // Name is required (e.g., "Bella")
        [Required]
        public string Name { get; set; } = string.Empty;

        // Foreign key linking the animal to a category (species)
        [Required]
        public int CategoryId { get; set; }

        // Navigation property to the Category this animal belongs to
        public Category? Category { get; set; }

        // Additional details about the animal
        public string Breed { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public int Age { get; set; }

        // Price stored as decimal
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        // Description used on details page
        public string Description { get; set; } = string.Empty;

        // Path or filename of the animal's image
        public string ImagePath { get; set; } = string.Empty;

        // Availability status (1 = available, 0 = not available)
        public bool IsAvailable { get; set; } = true;
    }
}
