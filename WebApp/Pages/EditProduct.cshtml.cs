using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages
{
    [Authorize]
    public class EditProductModel(ILogger<EditProductModel> logger, IProductService productService, IAuthService authService) : PageModel
    {
        private readonly ILogger<EditProductModel> _logger = logger;
        private readonly IProductService _productService = productService;
        private readonly IAuthService _authService = authService;

        [BindProperty]
        public ProductResponse? Product { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                var productResponse = await _productService.GetProductAsync(id);
                var firstProduct = productResponse.Products?.FirstOrDefault();

                if (productResponse.IsSuccessStatusCode && firstProduct != null && firstProduct.Id > 0)
                {
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

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid || Product == null)
            {
                return Page();
            }

            try
            {
                var token = await _authService.GetTokenAsync() ?? string.Empty;
                var productResponse = await _productService.UpdateProductAsync(Product.Id, Product, token);
                
                if (productResponse.IsSuccessStatusCode)
                {
                    return RedirectPermanent($"/product/{Product.Id}");
                }

                if (productResponse.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    ModelState.AddModelError("All", "Too many requests, try again later.");
                    return Page();
                }

                if (productResponse.StatusCode == HttpStatusCode.Forbidden || productResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("All", "Only Administrators can edit products. Please sign in as an administrator.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product with ID {Product.Id}.");
            }

            ModelState.AddModelError("All", "Server error. Please contact administrator.");
            return Page();
        }
    }
}
