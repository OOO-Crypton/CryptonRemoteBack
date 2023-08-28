namespace CryptonRemoteBack.Model.Models
{
    public class FlightSheetModel
    {
        public string Name { get; set; } = string.Empty;
        public string ExtendedConfig { get; set; } = string.Empty;
        public int? MinerId { get; set; } = null;
        public int? WalletId { get; set; } = null;
        public int? PoolId { get; set; } = null;
    }
}
