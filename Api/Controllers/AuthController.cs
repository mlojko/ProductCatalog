using Microsoft.AspNetCore.Mvc;
using Api.Models.Auth;
using Api.Infrastructure.Services;
using Api.Infrastructure.Helpers;
using Asp.Versioning;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController(IUserService userService, IPasswordHasherHelper passwordHasher, IAuthService authService) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly IPasswordHasherHelper _passwordHasher = passwordHasher;
        private readonly IAuthService _authService = authService;

        [HttpPost("Login", Name = "Login")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthRequest request)
        {
            var user = await _userService.GetByUsername(request.Username);
            if (user == null || !_passwordHasher.VerifyPassword(request.Password, user.Password))
            {
                return Unauthorized("Invalid username or password.");
            }
            var token = _authService.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }
    }
}
