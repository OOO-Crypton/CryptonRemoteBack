using System.ComponentModel.DataAnnotations;

namespace CryptonRemoteBack.Model.Models
{
    public class ChangeUserPasswordModel
    {
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? OldPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? NewPassword { get; set; }
    }
}
