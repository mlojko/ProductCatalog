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
            var products = new List<Product>
                {
                    new Product() { Id = 1, Name = "Product 1", Description = "Description 1", Price = 10.00m, Stock = 100 }
                };

            for (int i = 2; i <= 31; i++)
            {
                products.Add(new Product()
                {
                    Id = i,
                    Name = $"Product {i}",
                    Description = $"Description {i}",
                    Price = 10.00m * i,
                    Stock = 100 * i
                });
            }

            modelBuilder.Entity<Product>().HasData(products);
        }
    }
}
