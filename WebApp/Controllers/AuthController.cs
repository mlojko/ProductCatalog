using Microsoft.AspNetCore.Mvc;
using WebApp.Services;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Controllers
{
    public class AuthController(IAuthService authService) : Controller
    {
        private readonly IAuthService _authService = authService;

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            //_logger.LogInformation("User {Name} logged out at {Time}.", User.Identity.Name, DateTime.UtcNow);

            await _authService.LogOutAsync();

            return RedirectToPage("/Index");
        }
    }
}
