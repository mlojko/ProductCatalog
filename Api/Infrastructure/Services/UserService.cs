using Api.Models.Auth;
using Api.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Services
{
    public class UserService(ProductsDbContext context) : IUserService
    {
        private readonly ProductsDbContext _context = context;

        public async Task<User?> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public Task<User?> GetByUsername(string userName)
        {
            return _context.Users.SingleOrDefaultAsync(u => u.Username == userName);
        }
    }
}
