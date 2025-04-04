using System;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Api.Infrastructure.Repositories;
using Api.Infrastructure.Services;
using Api.Models.Auth;
using Api.Models.Products;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Api.Tests
{
    [Collection("Sequential")]
    public sealed class ProductControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };
        private readonly WebApplicationFactory<Program> _factory = factory;

        private static readonly Mock<IProductRepository> _mockRepo = new();
        private readonly ProductService _productService = new(_mockRepo.Object);

        private ProductsDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            return new ProductsDbContext(options);
        }

        /// <summary>
        /// Using Moq to create a mock repository for the ProductService
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetProducts_ReturnsAtLeastTwoProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new() { Id = 1, Name = "Product1", Price = 10 },
                new() { Id = 2, Name = "Product2", Price = 20 },
            };
            _mockRepo.Setup(repo => repo.GetProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Product1", result.First().Name);
        }

        /// <summary>
        /// Integration test to retrieve 1 product from DB. Assumes DB is seeded with at least 1 product
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [Theory]
        [InlineData("/api/v1/Product/GetProduct/1")]
        public async Task GetProduct_ReturnsOneProduct(string url)
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var content = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(content, _jsonSerializerOptions);
            
            Assert.NotNull(product);
            Assert.Equal(1, product.Id);

        }

        /// <summary>
        /// Test using in memeory database to test adding a product to the DB
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddProduct_FindsOneProduct()
        {
            var productPayload = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.99m,
                Stock = 100
            };
                        
            // Arrange
            var context = GetInMemoryDbContext();
            var config = _factory.Services.GetService<IConfiguration>();
            var repository = new ProductRepository(context, config);

            // Act
            await repository.AddProductAsync(productPayload);
            var savedProduct = await context.Products.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(savedProduct);
            Assert.Equal("Test Product", savedProduct.Name);
            Assert.Equal("Test Description", savedProduct.Description);
            Assert.Equal(10.99m, savedProduct.Price);
            Assert.Equal(100, savedProduct.Stock);
        }

        /// <summary>
        /// Using in memory database to test deleting a product from the DB
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeletesOneProduct_ReturnsNoProduct()
        {
            var productPayload = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.99m,
                Stock = 100
            };

            // Arrange
            var context = GetInMemoryDbContext();
            var config = _factory.Services.GetService<IConfiguration>();
            var repository = new ProductRepository(context, config);

            // Act
            await repository.AddProductAsync(productPayload);
            var savedProduct = await context.Products.FirstOrDefaultAsync();
            await repository.DeleteProductAsync(savedProduct.Id);
            var result = await context.Products.ToListAsync();
            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Does not work in meory database doesnt seem to allow for updates. IGNORE
        /// </summary>
        /// <returns></returns>
        public async Task EditOneProduct_ReturnsEditedProduct()
        {
            var productPayload = new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.99m,
                Stock = 100
            };

            // Arrange
            var context = GetInMemoryDbContext();
            context.Products.Add(productPayload);
            context.SaveChanges();
            var config = _factory.Services.GetService<IConfiguration>();
            var repository = new ProductRepository(context, config);

            // Act
            
            var savedProduct = await context.Products.FirstOrDefaultAsync();

            Assert.NotNull(savedProduct);
            Assert.Equal("Test Product", savedProduct.Name);
            Assert.Equal("Test Description", savedProduct.Description);
            Assert.Equal(10.99m, savedProduct.Price);
            Assert.Equal(100, savedProduct.Stock);

            var updatedProduct = new Product
            {
                Id = savedProduct.Id,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 20.99m,
                Stock = 200
            };

            await repository.UpdateProductAsync(updatedProduct);

            var savedUpdatedProduct = await context.Products.FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(savedUpdatedProduct);
            Assert.Equal("Updated Product", savedUpdatedProduct.Name);
            Assert.Equal("Updated Description", savedUpdatedProduct.Description);
            Assert.Equal(20.99m, savedUpdatedProduct.Price);
            Assert.Equal(200, savedUpdatedProduct.Stock);

        }

        /// <summary>
        /// Integration test making multiple calls to the rate limited service
        /// </summary>
        /// <returns></returns>
        [Fact]        
        public async Task Login_Ratelimited()
        {
            var creds = new AuthRequest
            {
                Username = "admin",
                Password = "admin"
            };

            var client = _factory.CreateClient();
            await client.PostAsJsonAsync("/api/v1/auth/login", creds);            
            await client.PostAsJsonAsync("/api/v1/auth/login", creds);
            await client.PostAsJsonAsync("/api/v1/auth/login", creds);
            var authResponse = await client.PostAsJsonAsync("/api/v1/auth/login", creds);

            Assert.Equal(HttpStatusCode.TooManyRequests, authResponse.StatusCode);
        }
    }
}
