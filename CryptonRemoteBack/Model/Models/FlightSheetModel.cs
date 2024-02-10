namespace CryptonRemoteBack.Model.Models
{
    public class FlightSheetModel
    {
        public string Name { get; set; } = string.Empty;
        public string? ExtendedConfig { get; set; } = null;
        public int? MinerId { get; set; } = null;
        public int? WalletId { get; set; } = null;
        public string PoolAddress { get; set; } = string.Empty;
        public double Hashrate { get; set; }
    }
}
