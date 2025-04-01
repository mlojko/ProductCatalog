using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using WebApp.Models;

namespace WebApp.Pages
{
    public class IndexModel(IOptions<AppSettings> _configuration, IHttpClientFactory clientFactory) : PageModel
    {
        private readonly AppSettings _appSettings = _configuration.Value;
        private readonly IHttpClientFactory _clientFactory = clientFactory;

        public List<ProductResponse>? Products { get; private set; }

        public async Task OnGet()
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{_appSettings.ApiBaseUrl}/Product/GetProducts")
            {
                Headers =
                {
                    { HeaderNames.Accept, "*/*" }
                }
            };

            var httpClient = _clientFactory.CreateClient();
            var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            Products = JsonSerializer.Deserialize<List<ProductResponse>>(responseString, options);
        }
    }
}
