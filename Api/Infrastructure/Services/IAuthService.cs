using Api.Models.Auth;

namespace Api.Infrastructure.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
    }
}
