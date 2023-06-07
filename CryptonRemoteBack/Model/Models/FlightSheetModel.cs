namespace CryptonRemoteBack.Model.Models
{
    public class FlightSheetModel
    {
        public string Name { get; set; } = string.Empty;
        public string ExtendedConfig { get; set; } = string.Empty;
        public Guid MinerId { get; set; } = Guid.Empty;
        public Guid WalletId { get; set; } = Guid.Empty;
        public Guid PoolId { get; set; } = Guid.Empty;
    }
}
