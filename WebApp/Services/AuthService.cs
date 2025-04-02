using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using WebApp.Models;

namespace WebApp.Services
{
    public class AuthService(IHttpContextAccessor httpContextAccessor, IOptions<AppSettings> _configuration, IHttpClientFactory clientFactory) : IAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IHttpClientFactory _clientFactory = clientFactory;
        private readonly AppSettings _appSettings = _configuration.Value;

        public async Task<string> GetTokenAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");
            var authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (authenticateResult.Succeeded && authenticateResult.Properties != null)
            {
                return authenticateResult.Properties.GetTokenValue("jwt") ?? string.Empty;
            }

            return string.Empty;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");
            var authenticateResult = await httpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return authenticateResult.Succeeded;
        }

        public async Task LogOutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<bool> LogIn(string username, string password)
        {
            var httpClient = _clientFactory.CreateClient();
            var httpResponseMessage = await httpClient.PostAsJsonAsync($"{_appSettings.ApiBaseUrl}/Auth/Login", new { username, password });
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var authResult = JsonSerializer.Deserialize<AuthResponse>(await httpResponseMessage.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (authResult == null)
                {
                    return false;
                }

                var claims = new List<Claim>
                    {
                        new(ClaimTypes.Name, username)
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var ap = new AuthenticationProperties() { IsPersistent = true };
                ap.StoreTokens([
                    new AuthenticationToken() {
                        Name = "jwt",
                        Value = authResult.Token }
                ]);

                var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("HttpContext is null");
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), ap);

                return true;
            }
            
            return false;
        }
    }
}
