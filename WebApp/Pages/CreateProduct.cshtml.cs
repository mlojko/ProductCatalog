using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using WebApp.Services;

namespace WebApp.Pages
{
    [Authorize]
    public class CreateProductModel(ILogger<CreateProductModel> logger, IProductService productService, IAuthService authService) : PageModel
    {
        private readonly ILogger<CreateProductModel> _logger = logger;
        private readonly IProductService _productService = productService;
        private readonly IAuthService _authService = authService;

        [BindProperty]
        public ProductPayload Product { get; set; } = new ProductPayload();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var token = await _authService.GetTokenAsync() ?? string.Empty;
                var productResponse = await _productService.AddProductAsync(Product, token);
                var firstProduct = productResponse.Products?.FirstOrDefault();

                if (productResponse.IsSuccessStatusCode && firstProduct != null && firstProduct.Id > 0)
                {
                    return RedirectPermanent($"/product/{firstProduct.Id}");
                }

                if (productResponse.StatusCode == System.Net.HttpStatusCode.Forbidden || productResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("All", "Only Administrators can create new products. Please sign in as an administrator.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product.");
            }

            ModelState.AddModelError("All", "Server error. Please contact administrator.");
            return Page();
        }
    }
}
