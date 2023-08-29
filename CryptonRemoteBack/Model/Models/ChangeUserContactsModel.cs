using System.ComponentModel.DataAnnotations;

namespace CryptonRemoteBack.Model.Models
{
    public class ChangeUserEmailModel
    {
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }
    }


    public class ChangeUserPhoneModel
    {

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string? PhoneNumber { get; set; }
    }
}
