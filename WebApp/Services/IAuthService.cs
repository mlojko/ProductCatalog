namespace WebApp.Services
{
    public interface IAuthService
    {
        Task<string> GetTokenAsync();
        Task<bool> IsAuthenticatedAsync();
        Task LogOutAsync();
        Task<HttpResponseMessage> LogIn(string username, string password);
    }
}