using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CryptonRemoteBack.Domain.CoinsDatabase
{
    public class Coin
    {
        [Key] public Guid Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Номер монеты на сайте
        /// </summary>
        public int CoinNumber { get; set; }

        /// <summary>
        /// Тэг
        /// </summary>
        public string Tag { get; set; } = null!;

        /// <summary>
        /// Используемый алгоритм
        /// </summary>
        public Algorithm Algorithm { get; set; } = null!;

        /// <summary>
        /// Данные мониторинга
        /// </summary>
        public List<Monitoring> Monitorings { get; set; } = new List<Monitoring>();

    }
}
