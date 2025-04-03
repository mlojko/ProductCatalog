using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Api.Models.Auth;
using Api.Models.Products;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.Tests
{
    public sealed class ProductControllerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory;
        private static JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        [Theory]
        [InlineData("/api/v1/Product/GetProducts")]
        public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<Product>>(content, _jsonSerializerOptions);

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.NotNull(products);
            Assert.True(products.Count > 1);
        }

        [Theory]
        [InlineData("/api/v1/Product/GetProduct/1")]
        public async Task GetProduct_ReturnsOneProduct(string url)
        {

            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var content = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(content, _jsonSerializerOptions);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.NotNull(product);
            Assert.Equal(1, product.Id);
        }

        [Theory]
        [InlineData("/api/v1/Product/AddProduct")]
        public async Task AddProduct_DeletesOneProduct(string url)
        {

            // Arrange
            var client = _factory.CreateClient();
            var productPayload = new ProductPayload
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.99m,
                Stock = 100
            };
            var creds = new AuthRequest
            {
                Username = "admin",
                Password = "admin"
            };

            // Act
            var authResponse = await client.PostAsJsonAsync("/api/v1/auth/login", creds);
            authResponse.EnsureSuccessStatusCode();
            var authResponseString = await authResponse.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<AuthResponse>(authResponseString, _jsonSerializerOptions);
            
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
            var response = await client.PostAsJsonAsync(url, productPayload);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var content = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(content, _jsonSerializerOptions);

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            Assert.NotNull(product);
            Assert.Equal("Test Product", product.Name);
            Assert.Equal(10.99m, product.Price);
            Assert.Equal(100, product.Stock);

            var deleteResponse = await client.DeleteAsync($"/api/v1/Product/DeleteProduct/{product.Id}");
            deleteResponse.EnsureSuccessStatusCode(); // Status Code 200-299

            var responseCheck = await client.GetAsync($"/api/v1/Product/GetProduct/{product.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, responseCheck.StatusCode);
        }
    }
}
