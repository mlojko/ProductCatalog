namespace WebApp.Services
{
    public interface IAuthService
    {
        Task<string> GetTokenAsync();
        Task<bool> IsAuthenticatedAsync();
        Task LogOutAsync();
        Task<bool> LogIn(string username, string password);
    }
}