using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptonRemoteBack.Domain
{

    /// <summary>
    /// Пул
    /// </summary>
    [Table("Pools")]
    public class Pool
    {
        [Key] public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = string.Empty;
        public List<PoolAddress> PoolAddresses { get; set; } = new();

        public List<FlightSheet> FlightSheets { get; set; } = new();

    }
}
