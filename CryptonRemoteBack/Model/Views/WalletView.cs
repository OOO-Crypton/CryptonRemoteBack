using CryptonRemoteBack.Domain;
using CryptonRemoteBack.Infrastructure.Migrations;

namespace CryptonRemoteBack.Model.Views
{
    public class WalletView
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public UserView User { get; set; } = null!;
        public CurrencyView Currency { get; set; } = null!;

        public WalletView(Wallet? input)
        {
            if (input == null) return;
            Id = input.Id;
            Name = input.Name;
            Address = input.Address;
            User = new UserView(input.User);
            Currency = new CurrencyView(input.Currency);
        }
    }
}
