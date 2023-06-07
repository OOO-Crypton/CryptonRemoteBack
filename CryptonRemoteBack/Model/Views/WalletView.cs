using CryptonRemoteBack.Domain;

namespace CryptonRemoteBack.Model.Views
{
    public class WalletView
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public UserView User { get; set; } = null!;
        public CurrencyView Currency { get; set; } = null!;

        public WalletView(Wallet wallet)
        {
            Id = wallet.Id;
            Name = wallet.Name;
            Address = wallet.Address;
            User = new UserView(wallet.User);
            Currency = new CurrencyView(wallet.Currency);
        }
    }
}
