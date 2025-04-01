using Api.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace Api.Models.Products
{
    public class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

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

            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, Username = "admin", Password = "$2a$11$M5oW82FBNXNkmXNEoEo7hepd.Nnz2vkFEz/riZUKSgH8T6MgrAxda" },
                new User() { Id = 2, Username = "viewer", Password = "$2a$11$.sMJ./zBiI4quAi6F4mqwurhB8H3IlTYBcDVCo/O1nREh7dBkF1M." }
            );
        }
    }
}
