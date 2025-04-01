using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using WebApp.Models;

namespace WebApp.Pages
{
    public class ProductModel(IOptions<AppSettings> _configuration, IHttpClientFactory clientFactory) : PageModel
    {
        private readonly AppSettings _appSettings = _configuration.Value;
        private readonly IHttpClientFactory _clientFactory = clientFactory;

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
                var httpClient = _clientFactory.CreateClient();
                var httpResponseMessage = await httpClient.DeleteAsync($"{_appSettings.ApiBaseUrl}/Product/DeleteProduct/{Product.Id}");
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("All", "Server error. Please contact administrator.");
                    return Page();
                }
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError("All", "Server error. Please contact administrator.");
                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}
