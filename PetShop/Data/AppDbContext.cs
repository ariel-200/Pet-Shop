using Microsoft.EntityFrameworkCore;
using PetShop.Models;

namespace PetShop.Data
{
    // Represents the connection between the application and the database.
    // EF Core uses this class to query and save data.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets represent tables in the database.
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Animal> Animals => Set<Animal>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map C# class names to actual SQLite table names
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Animal>().ToTable("Animal");
        }
    }
}
