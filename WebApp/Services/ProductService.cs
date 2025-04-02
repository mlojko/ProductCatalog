using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using WebApp.Models;

namespace WebApp.Services
{
    public class ProductService(IOptions<AppSettings> _configuration, IHttpClientFactory clientFactory) : IProductService
    {
        private readonly AppSettings _appSettings = _configuration.Value;
        private readonly IHttpClientFactory _clientFactory = clientFactory;

        private static JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public async Task<ProductHttpResponse> AddProductAsync(ProductPayload product, string token)
        {
            var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"{_appSettings.ApiBaseUrl}/Product/AddProduct", product);
            return await ParseSingleProductHttpRespnse(httpResponseMessage);
        }        

        public async Task<ProductHttpResponse> DeleteProductAsync(int id, string token)
        {
            var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var httpResponseMessage = await httpClient.DeleteAsync($"{_appSettings.ApiBaseUrl}/Product/DeleteProduct/{id}");

            return new ProductHttpResponse
            {
                IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode,
                StatusCode = httpResponseMessage.StatusCode,
            };
        }

        public async Task<ProductHttpResponse> GetProductAsync(int id)
        {
            var httpClient = _clientFactory.CreateClient();
            var httpResponseMessage = await httpClient.GetAsync($"{_appSettings.ApiBaseUrl}/Product/GetProduct/{id}");
            
            return await ParseSingleProductHttpRespnse(httpResponseMessage);
        }

        public async Task<ProductHttpResponse> GetProductsAsync()
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
            var products = JsonSerializer.Deserialize<List<ProductResponse>>(responseString, _jsonSerializerOptions);

            return new ProductHttpResponse
            {
                Products = products,
                IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode,
                StatusCode = httpResponseMessage.StatusCode
            };
        }

        public async Task<ProductHttpResponse> UpdateProductAsync(int id, ProductResponse product, string token)
        {
            var httpClient = _clientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var httpResponseMessage = await httpClient.PutAsJsonAsync($"{_appSettings.ApiBaseUrl}/Product/UpdateProduct/{id}", product);
            
            return new ProductHttpResponse
            {
                IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode,
                StatusCode = httpResponseMessage.StatusCode,
            };
        }

        private static async Task<ProductHttpResponse> ParseSingleProductHttpRespnse(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var responseString = await httpResponseMessage.Content.ReadAsStringAsync();
                var productResponse = JsonSerializer.Deserialize<ProductResponse>(responseString, _jsonSerializerOptions);
                if (productResponse != null && productResponse.Id > 0)
                {
                    return new ProductHttpResponse
                    {
                        IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode,
                        StatusCode = httpResponseMessage.StatusCode,
                        Products = [productResponse]
                    };
                }
            }

            return new ProductHttpResponse
            {
                IsSuccessStatusCode = httpResponseMessage.IsSuccessStatusCode,
                StatusCode = httpResponseMessage.StatusCode,
            };
        }
    }
}
