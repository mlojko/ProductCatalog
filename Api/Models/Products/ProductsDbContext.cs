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
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

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

            // Configure the UserRole entity as a join table for User and Role.
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId }); // Composite primary key.

            //Defines the many-to-many relationship between User and Role.
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<User>().HasData(
                new User() { Id = 1, Username = "admin", Password = "$2a$11$M5oW82FBNXNkmXNEoEo7hepd.Nnz2vkFEz/riZUKSgH8T6MgrAxda" },
                new User() { Id = 2, Username = "viewer", Password = "$2a$11$.sMJ./zBiI4quAi6F4mqwurhB8H3IlTYBcDVCo/O1nREh7dBkF1M." }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = 1, Name = "Admin", Description = "Admin Role" },
                new Role() { Id = 2, Name = "Viewer", Description = "Viewer Role" }
            );

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole() { UserId = 1, RoleId = 1 },
                new UserRole() { UserId = 1, RoleId = 2 },
                new UserRole() { UserId = 2, RoleId = 2 }
            );
        }
    }
}
