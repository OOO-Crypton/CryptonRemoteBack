using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptonRemoteBack.Domain
{
    [Table("Miners")]
    public class Miner
    {
        [Key] public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Путь к файлу конфигурации
        /// </summary>
        public string MinerInfo { get; set; } = string.Empty;

        public List<FlightSheet> FlightSheets { get; set; } = new();
    }
}
