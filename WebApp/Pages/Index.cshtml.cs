using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages
{
    [Authorize]
    public class IndexModel(ILogger<IndexModel> logger, IProductService productService) : PageModel
    {
        private readonly ILogger<IndexModel> _logger = logger;
        private readonly IProductService _productService = productService;

        public List<ProductResponse>? Products { get; private set; }

        public async Task OnGet()
        {
            try
            {
                Products = (await _productService.GetProductsAsync()).Products;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                _logger.LogError(ex, "Error getting products.");
            }
        }
    }
}
