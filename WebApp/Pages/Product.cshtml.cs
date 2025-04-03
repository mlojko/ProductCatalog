using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages
{
    [Authorize]
    public class ProductModel(ILogger<ProductModel> logger, IProductService productService, IAuthService authService) : PageModel
    {
        private readonly ILogger<ProductModel> _logger = logger;
        private readonly IProductService _productService = productService;
        private readonly IAuthService _authService = authService;

        [BindProperty]
        public ProductResponse? Product { get; set; }
        public string? ReturnUrl { get; set; }

        public async Task<IActionResult> OnGet(int id, string? returnUrl = null)
        {
            try
            {
                var productResponse = await _productService.GetProductAsync(id);
                var firstProduct = productResponse.Products?.FirstOrDefault();

                if (productResponse.IsSuccessStatusCode && firstProduct != null && firstProduct.Id > 0)
                {
                    ReturnUrl = returnUrl;
                    Product = firstProduct;
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("All", "Server error. Please contact administrator.");
                _logger.LogError(ex, $"Error getting product with ID {id}.");
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {

            if (Product == null || Product.Id <= 0)
            {
                ModelState.AddModelError("All", "Product not found.");
                return Page();
            }

            try
            {
                var token = await _authService.GetTokenAsync() ?? string.Empty;
                var productResponse = await _productService.DeleteProductAsync(Product.Id, token);

                if (productResponse.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Index");
                }

                if (productResponse.StatusCode == HttpStatusCode.Forbidden || productResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("All", "Only administrators can delete products. Log in as an administrator.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product with ID {Product.Id}.");
            }

            ModelState.AddModelError("All", "Server error. Please contact administrator.");
            return Page();
        }
    }
}
