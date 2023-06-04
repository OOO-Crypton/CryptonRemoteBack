using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptonRemoteBack.Domain
{
    /// <summary>
    /// Полетный лист
    /// </summary>
    [Table("FlightSheets")]
    public class FlightSheet
    {
        [Key] public Guid Id { get; set; }

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
        public Pool Pool { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
