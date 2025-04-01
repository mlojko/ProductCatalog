using Api.Models.Auth;

namespace Api.Infrastructure.Services
{
    public interface IUserService
    {
        Task<User?> GetById(int id);
        Task<User?> GetByUsername(string userName);
    }
}