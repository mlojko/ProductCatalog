using Microsoft.EntityFrameworkCore;

namespace Api.Models.Products
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() {
                    Id = 1,
                    Name = "Product 1",
                    Description = "Description 1",
                    Price = 10.00m,
                    Stock = 100
                }
                );
        }
    }
}
