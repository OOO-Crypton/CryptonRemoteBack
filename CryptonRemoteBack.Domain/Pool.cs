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
    /// Пул
    /// </summary>
    [Table("Pools")]
    public class Pool
    {
        [Key] public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = string.Empty;
        public List<PoolAddress> PoolAddresses { get; set; } = new();

        public List<FlightSheet> FlightSheets { get; set; } = new();

    }
}
