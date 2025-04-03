using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages
{
    [Authorize]
    public class PagedProductsModel(ILogger<PagedProductsModel> logger, IProductService productService) : PageModel
    {
        private readonly ILogger<PagedProductsModel> _logger = logger;
        private readonly IProductService _productService = productService;

        public PagedProductsResponse? PagedProducts { get; private set; }

        public async Task OnGet(int pageNumber)
        {
            try
            {
                PagedProducts = await _productService.GetPagedProductsAsync(pageNumber);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                _logger.LogError(ex, "Error getting products.");
            }
        }
    }
}
