using System.Linq;
using Api.Infrastructure.Repositories;
using Api.Models.Products;
using Api.Models.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Api.Infrastructure.Services
{
    public class ProductService(ILogger<ProductService> logger, IProductRepository productRepository, IRedisRepository redisRepository, IOptions<AppSettings> configuration) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IRedisRepository _redisRepository = redisRepository;
        private readonly int _productsPerPage = configuration?.Value?.ProductsPerPage ?? 6;
        private readonly ILogger<ProductService> _logger = logger;

        private readonly object _AllProductsLock = new();
        private readonly object _PaginatedProductsLock = new();
        private readonly object _SingleProductsLock = new();

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            
            var cacheKey = "ProductService-GetProductsAsync-All";

            var res = await _redisRepository.GetString<IEnumerable<Product>>(cacheKey);
            if (res != null)
            {
                _logger.LogInformation("Cache hit for GetProductsAsync");
                return res;
            }

            lock(_AllProductsLock)
            {
                res = _redisRepository.GetString<IEnumerable<Product>>(cacheKey).Result;
                if (res != null)
                {
                    _logger.LogInformation("Cache hit for GetProductsAsync");
                    return res;
                }
                res = _productRepository.GetProductsAsync().Result;
                if (res != null)
                {
                    _logger.LogInformation("Cache miss for GetProductsAsync");
                    var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) };
                    _redisRepository.SetString(cacheKey, res, options).Wait();
                }
            }
            
            return res ?? [];
        }

        public async Task<PagedProductsResult> GetProductsAsync(int page)
        {
            page--; // Adjust for zero-based index
            if (page < 0)
            {
                page = 0; // Ensure page is not negative
            }
            var products = await GetProductsAsync() ?? [];

            return new PagedProductsResult
            {
                Products = [.. products.Skip(_productsPerPage * page).Take(_productsPerPage)],
                TotalCount = products.Count(),
                PageSize = _productsPerPage,
                CurrentPage = page + 1,
                TotalPages = (int)Math.Ceiling((double)products.Count() / _productsPerPage)
            };
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {            
            var cacheKey = $"ProductService-GetProductByIdAsync-{id}";
            Product? res = null;
            try
            {
                res = await _redisRepository.GetString<Product>(cacheKey);
                if (res != null)
                {
                    _logger.LogInformation("Cache hit for GetProductByIdAsync");
                    return res;
                }

                lock (_SingleProductsLock)
                {
                    res = _redisRepository.GetString<Product>(cacheKey).Result;
                    if (res != null)
                    {
                        _logger.LogInformation("Cache hit for GetProductByIdAsync");
                        return res;
                    }
                    res = _productRepository.GetProductByIdAsync(id).Result;
                    if (res != null)
                    {
                        _logger.LogInformation("Cache miss for GetProductByIdAsync");
                        var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) };
                        _redisRepository.SetString(cacheKey, res, options).Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product with ID {ProductId} from Redis", id);
            }
            return res;
        }

        public async Task AddProductAsync(Product product)
        {
            await _productRepository.AddProductAsync(product);
            //await InvalidateCache(-1);
        }

        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateProductAsync(product);
            //await InvalidateCache(product.Id);
        }

        public async Task DeleteProductAsync(int id)
        {
            await _productRepository.DeleteProductAsync(id);
            //await InvalidateCache(id);
        }

        private async Task InvalidateCache(int id)
        {
            if (id != -1)
            {
                await _redisRepository.DeleteKey($"ProductService-GetProductByIdAsync-{id}");
            }
            await _redisRepository.DeleteKey("ProductService-GetProductsAsync-All");
        }
    }
}
