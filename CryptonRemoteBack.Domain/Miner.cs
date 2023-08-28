using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptonRemoteBack.Domain
{
    [Table("Miners")]
    public class Miner
    {
        [Key] public int Id { get; set; }

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
