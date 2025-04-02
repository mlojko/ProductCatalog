using WebApp.Models;

namespace WebApp.Services
{
    public interface IProductService
    {
        Task<ProductHttpResponse> GetProductsAsync();
        Task<ProductHttpResponse> GetProductAsync(int id);
        Task<ProductHttpResponse> AddProductAsync(ProductPayload product, string token);
        Task<ProductHttpResponse> UpdateProductAsync(int id, ProductResponse product, string token);
        Task<ProductHttpResponse> DeleteProductAsync(int id, string token);
    }
}