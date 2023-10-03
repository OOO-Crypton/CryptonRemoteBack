using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CryptonRemoteBack.Domain.CoinsDatabase
{
    [Table("Monitorings")]
    public class Monitoring
    {
        [Key] public Guid Id { get; set; }

        /// <summary>
        /// Время парсинга
        /// </summary>
        public DateTime Date { get; set; }

        public double BlockReward { get; set; }
        public double LastBlock { get; set; }

        /// <summary>
        /// Сложность сети
        /// </summary>
        public double Difficulty { get; set; }

        public double Nethash { get; set; }

        /// <summary>
        /// Текущий курс монеты
        /// </summary>
        public double ExRate { get; set; }

        public double ExchangeRateVol { get; set; }
        /// <summary>
        /// Рыночная капитализация
        /// </summary>
        public double MarketCap { get; set; }

        public double PoolFee { get; set; }
        public double EstimatedRewards { get; set; }
        public double BtcRevenue { get; set; }

        public double Revenue { get; set; }
        public double Hashrate { get; set; }

        /// <summary>
        /// Связанная монета
        /// </summary>
        public Coin Coin { get; set; } = null!;
    }
}
