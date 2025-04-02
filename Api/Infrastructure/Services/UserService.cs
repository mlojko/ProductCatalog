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
            return await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Id == id);
        }

        public Task<User?> GetByUsername(string userName)
        {
            return _context.Users.Include(u => u.UserRoles)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .SingleOrDefaultAsync(u => u.Username == userName);
        }
    }
}
