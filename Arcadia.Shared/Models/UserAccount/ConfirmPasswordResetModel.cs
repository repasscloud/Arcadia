using System.ComponentModel.DataAnnotations;

namespace Arcadia.Shared.Models.UserAccount
{
    public class ConfirmResetPasswordModel
    {
        [Required]
        public required string Token { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password should be minimum 6 characters")]
        public required string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Confirm Password does not match")]
        public required string ConfirmPassword { get; set; }
    }
}
