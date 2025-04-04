using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Services;

namespace WebApp.Pages
{
    public class LoginModel(IAuthService authService) : PageModel
    {
        private readonly IAuthService _authService = authService;

        [BindProperty]
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var respMessage = await _authService.LogIn(Username, Password);
            if (respMessage.IsSuccessStatusCode)
            {
                return LocalRedirect(ReturnUrl);
            }

            if (respMessage.StatusCode == HttpStatusCode.TooManyRequests)
            {
                ModelState.AddModelError("All", "Too many requests, try again later.");
                return Page();
            }


            ModelState.AddModelError("All", "Invalid login attempt.");
            return Page();
        }
    }
}
