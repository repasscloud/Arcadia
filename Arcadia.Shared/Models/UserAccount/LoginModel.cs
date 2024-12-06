using System.ComponentModel.DataAnnotations;

namespace Arcadia.Shared.Models.UserAccount
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
