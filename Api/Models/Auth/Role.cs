using System.ComponentModel.DataAnnotations;

namespace Api.Models.Auth
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Role name is required.")]
        [MaxLength(50, ErrorMessage = "Role name must be less than or equal to 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250, ErrorMessage = "Description must be less than or equal to 250 characters.")]
        public string? Description { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; } = [];
    }
}
