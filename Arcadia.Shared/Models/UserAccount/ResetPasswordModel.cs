using System.ComponentModel.DataAnnotations;

namespace Arcadia.Shared.Models.UserAccount
{
    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
