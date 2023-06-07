using CryptonRemoteBack.Domain;

namespace CryptonRemoteBack.Model.Views
{
    public class UserView
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public UserView(ApplicationUser input)
        {
            if (input == null) return;

            Id = Guid.Parse(input.Id);
            UserName = input.UserName;
            Email = input.Email;
            PhoneNumber = input.PhoneNumber;
        }
    }
}
