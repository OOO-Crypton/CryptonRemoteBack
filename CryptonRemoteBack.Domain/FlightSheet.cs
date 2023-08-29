﻿using System.ComponentModel.DataAnnotations;
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

        public bool IsActive { get; set; }

        public Miner Miner { get; set; } = null!;
        public Wallet Wallet { get; set; } = null!;
        public Pool Pool { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
