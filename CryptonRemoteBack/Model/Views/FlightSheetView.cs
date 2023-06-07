using CryptonRemoteBack.Domain;

namespace CryptonRemoteBack.Model.Views
{
    public class FlightSheetView
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ExtendedConfig { get; set; } = string.Empty;

        public MinerView Miner { get; set; } = null!;
        public WalletView Wallet { get; set; } = null!;
        public PoolView Pool { get; set; } = null!;
        public UserView User { get; set; } = null!;

        public FlightSheetView(FlightSheet? input)
        {
            if (input == null) return;
            Id = input.Id;
            Name = input.Name;
            ExtendedConfig = input.ExtendedConfig;
            Miner = new MinerView(input.Miner);
            Wallet = new WalletView(input.Wallet);
            Pool = new PoolView(input.Pool);
            User = new UserView(input.User);
        }
    }
}
