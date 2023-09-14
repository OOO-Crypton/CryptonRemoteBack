using CryptonRemoteBack.Domain;

namespace CryptonRemoteBack.Model.Views
{
    public class FlightSheetView
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ExtendedConfig { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public MinerView Miner { get; set; } = null!;
        public WalletView Wallet { get; set; } = null!;
        public string PoolAddress { get; set; } = null!;
        public UserView User { get; set; } = null!;

        public FlightSheetView(FlightSheet? input, bool isActive)
        {
            if (input == null) return;
            Id = input.Id;
            Name = input.Name;
            ExtendedConfig = input.ExtendedConfig;
            IsActive = isActive;
            Miner = new MinerView(input.Miner);
            Wallet = new WalletView(input.Wallet);
            PoolAddress = input.PoolAddress;
            User = new UserView(input.User);
        }
    }
}
