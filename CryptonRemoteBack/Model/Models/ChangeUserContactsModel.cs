using System.ComponentModel.DataAnnotations;

namespace CryptonRemoteBack.Model.Models
{
    public class ChangeUserContactsModel
    {

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
