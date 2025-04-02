using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Pages
{
    [Authorize]
    public class ProductModel(IOptions<AppSettings> _configuration, IHttpClientFactory clientFactory, IAuthService authService) : PageModel
    {
        private readonly AppSettings _appSettings = _configuration.Value;
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly IAuthService _authService = authService;

        [BindProperty]
        public ProductResponse? Product { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var idVal = Request.RouteValues["id"];
            if (!int.TryParse(idVal?.ToString(), out var id))
            {
                return NotFound();
            }
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_appSettings.ApiBaseUrl}/Product/GetProduct/{id}")
            {
                Headers =
                {
                    { HeaderNames.Accept, "*/*" }
                }
            };
            try
            {
                var httpClient = _clientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                    Product = JsonSerializer.Deserialize<ProductResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
            }
            catch (HttpRequestException)
            {
                return NotFound();
            }

            return Product == null ? NotFound() : Page();
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
                var httpClient = _clientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var httpResponseMessage = await httpClient.DeleteAsync($"{_appSettings.ApiBaseUrl}/Product/DeleteProduct/{Product.Id}");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToPage("/Index");
                }

                if (httpResponseMessage.StatusCode == HttpStatusCode.Forbidden)
                {
                    ModelState.AddModelError("All", "Only administrators can delete products. Log in as administrator.");
                } else
                {
                    ModelState.AddModelError("All", "Server error. Please contact administrator.");
                }
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("All", "Server error. Please contact administrator.");
            }
            return Page();
        }
    }
}
