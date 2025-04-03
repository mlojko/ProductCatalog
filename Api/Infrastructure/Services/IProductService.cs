using Api.Models.Products;
namespace Api.Infrastructure.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<PagedProductsResult> GetProductsAsync(int page);
        Task<Product?> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
