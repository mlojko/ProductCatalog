using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using WebApp.Services;

namespace WebApp.Pages
{
    [Authorize]
    public class CreateProductModel(IOptions<AppSettings> _configuration, IHttpClientFactory clientFactory, IAuthService authService) : PageModel
    {
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly AppSettings _appSettings = _configuration.Value;
        private readonly IAuthService _authService = authService;

        [BindProperty]
        public ProductPayload? Product { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            ProductResponse? productResponse;            
            try
            {
                var token = await _authService.GetTokenAsync() ?? string.Empty;

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };                
                var httpClient = _clientFactory.CreateClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var httpResponseMessage = await httpClient.PostAsJsonAsync($"{_appSettings.ApiBaseUrl}/Product/AddProduct", Product);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                    productResponse = JsonSerializer.Deserialize<ProductResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (productResponse == null || productResponse.Id <= 0)
                    {
                        ModelState.AddModelError("All", "Server error. Please contact administrator.");
                        return Page();
                    }
                } 
                else
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

            return RedirectPermanent($"/product/{productResponse.Id}");
        }
    }
}
