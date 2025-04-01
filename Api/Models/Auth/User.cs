using System.ComponentModel.DataAnnotations;

namespace Api.Models.Auth
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [MaxLength(100, ErrorMessage = "Email must be less than or equal to 100 characters.")]
        public string Username { get; set; } = string.Empty;
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [MaxLength(100, ErrorMessage = "Password must be less than or equal to 100 characters.")]
        public string Password { get; set; } = string.Empty;
    }
}
