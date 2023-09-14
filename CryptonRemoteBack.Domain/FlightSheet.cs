using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptonRemoteBack.Domain
{
    /// <summary>
    /// Полетный лист
    /// </summary>
    [Table("FlightSheets")]
    public class FlightSheet
    {
        [Key] public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Командная строка
        /// </summary>
        public string ExtendedConfig { get; set; } = string.Empty;

        public Miner Miner { get; set; } = null!;
        public Wallet Wallet { get; set; } = null!;
        public string PoolAddress { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
    }
}
