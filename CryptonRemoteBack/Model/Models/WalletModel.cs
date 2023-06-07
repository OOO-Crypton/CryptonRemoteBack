namespace CryptonRemoteBack.Model.Models
{
    public class WalletModel
    {
        public string? Name { get; set; } = string.Empty;

        public string? Address { get; set; } = string.Empty;

        public Guid CurrencyId { get; set; } = Guid.Empty;
    }
}
