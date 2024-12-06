using System.ComponentModel.DataAnnotations;

namespace Arcadia.Shared.Models.UserAccount
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password should be minimum 6 characters")]
        public required string Password { get; set; } = null!;

        [Required]
        [Compare("Password", ErrorMessage = "Confirm Password does not match")]
        public required string ConfirmPassword { get; set; }
    }
}
