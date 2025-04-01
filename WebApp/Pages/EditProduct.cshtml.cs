using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using WebApp.Models;

namespace WebApp.Pages
{
    public class EditProductModel(IOptions<AppSettings> _configuration, IHttpClientFactory clientFactory) : PageModel
    {
        private readonly AppSettings _appSettings = _configuration.Value;
        private readonly IHttpClientFactory _clientFactory = clientFactory;

        [BindProperty]
        public ProductResponse? Product { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            var httpClient = _clientFactory.CreateClient();
            var httpResponseMessage = await httpClient.GetAsync($"{_appSettings.ApiBaseUrl}/Product/GetProduct/{id}");
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                Product = JsonSerializer.Deserialize<ProductResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (Product == null)
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid || Product == null)
            {
                return Page();
            }

            try
            {
                var httpClient = _clientFactory.CreateClient();
                var httpResponseMessage = await httpClient.PutAsJsonAsync($"{_appSettings.ApiBaseUrl}/Product/UpdateProduct/{Product.Id}", Product);
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

            return RedirectPermanent($"/product/{Product.Id}");
        }
    }
}
