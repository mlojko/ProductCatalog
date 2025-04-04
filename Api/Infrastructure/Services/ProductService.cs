using Api.Infrastructure.Repositories;
using Api.Models.Products;
using Api.Models.Settings;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Api.Infrastructure.Services
{
    public class ProductService(ILogger<ProductService> logger, IProductRepository productRepository, IRedisRepository redisRepository, IOptions<AppSettings> appSettings) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IRedisRepository _redisRepository = redisRepository;
        private readonly AppSettings _appSettings = appSettings.Value;
        private readonly ILogger<ProductService> _logger = logger;

        private readonly object _AllProductsLock = new();
        private readonly object _SingleProductsLock = new();


        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            try
            {
                if (_appSettings.Caching)
                {
                    return await GetProductsFromCache();
                }

                return await _productRepository.GetProductsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching products, cache enabled: {_appSettings.Caching}");
            }

            return [];
        }

        // page is 1-based
        public async Task<PagedProductsResult> GetProductsAsync(int page)
        {
            page--; // Adjust for zero-based index
            if (page < 0)
            {
                page = 0; // Ensure page is not negative
            }
            try
            {
                if (_appSettings.Caching)
                {
                    return await GetProductsFromCache(page);
                }
                return await _productRepository.GetProductsAsync(page);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching paginated products, cache enabled: {_appSettings.Caching}");
            }
            return new PagedProductsResult
            {
                Products = [],
                PageSize = _appSettings.ProductsPerPage,
                CurrentPage = page + 1,
            };
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                if (_appSettings.Caching)
                {
                    return await GetProductByIdFromCache(id);
                }
                return await _productRepository.GetProductByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching product with ID {id}, cache enabled: {_appSettings.Caching}");
            }

            return null;
        }

        public async Task AddProductAsync(Product product)
        {
            try
            {
                await _productRepository.AddProductAsync(product);
                if (_appSettings.Caching)
                {
                    await InvalidateCache(-1);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while adding product, cache enabled: {_appSettings.Caching}");
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                await _productRepository.UpdateProductAsync(product);
                if (_appSettings.Caching)
                {
                    await InvalidateCache(product.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating product with ID {product.Id}, cache enabled: {_appSettings.Caching}");
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
                if (_appSettings.Caching)
                {
                    await InvalidateCache(id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting product with ID {id}, cache enabled: {_appSettings.Caching}");
            }
        }

        private async Task<IEnumerable<Product>> GetProductsFromCache()
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
                    var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(_appSettings.CacheDuration) };
                    _redisRepository.SetString(cacheKey, res, options).Wait();
                }
            }
            
            return res ?? [];
        }

        
        private async Task<PagedProductsResult> GetProductsFromCache(int page)
        {
            var products = await GetProductsFromCache() ?? [];

            return new PagedProductsResult
            {
                Products = [.. products.Skip(_appSettings.ProductsPerPage * page).Take(_appSettings.ProductsPerPage)],
                TotalCount = products.Count(),
                PageSize = _appSettings.ProductsPerPage,
                CurrentPage = page + 1,
                TotalPages = (int)Math.Ceiling((double)products.Count() / _appSettings.ProductsPerPage)
            };
        }

        private async Task<Product?> GetProductByIdFromCache(int id)
        {            
            var cacheKey = $"ProductService-GetProductByIdAsync-{id}";

            var res = await _redisRepository.GetString<Product>(cacheKey);
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
                    var options = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(_appSettings.CacheDuration) };
                    _redisRepository.SetString(cacheKey, res, options).Wait();
                }
            }

            return res;
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
