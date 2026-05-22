using System.ComponentModel.DataAnnotations;

namespace GymApp.Core.DTOs
{
    public class RegisterDto
    {
        // [Required] means this field must be provided
        // [EmailAddress] validates it's a proper email format
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        // Password must be at least 6 characters
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        // Phone is optional (nullable with ?)
        public string? Phone { get; set; }
    }
}