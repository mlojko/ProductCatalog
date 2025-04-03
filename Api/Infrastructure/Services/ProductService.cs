using Api.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Services
{
    public class ProductService(ProductsDbContext context, IConfiguration configuration) : IProductService
    {
        private readonly ProductsDbContext _context = context;
        private readonly int _productsPerPage = configuration.GetValue<int>("ProductsPerPage", 6);

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<PagedProductsResult> GetProductsAsync(int page)
        {
            page--; // Adjust for zero-based index
            if (page < 0)
            {
                page = 0; // Ensure page is not negative
            }
            var total = await _context.Products.CountAsync();
            var products = await _context.Products.Skip(page * _productsPerPage).Take(_productsPerPage).ToListAsync();

            return new PagedProductsResult
            {
                Products = products,
                TotalCount = total,
                PageSize = _productsPerPage,
                CurrentPage = page + 1,
                TotalPages = (int)Math.Ceiling((double)total / _productsPerPage)
            };
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
