using CryptonRemoteBack.Domain.CoinsDatabase;

namespace CryptonRemoteBack.Model.Views
{
    public class MonitoringView
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double BlockReward { get; set; }
        public double LastBlock { get; set; }
        public double Difficulty { get; set; }
        public double Nethash { get; set; }
        public double ExRate { get; set; }
        public double ExchangeRateVol { get; set; }
        public double MarketCap { get; set; }
        public double PoolFee { get; set; }
        public double DailyEmission { get; set; }
        public string Coin { get; set; }

        public MonitoringView(Monitoring input)
        {
            Id = input.Id;
            Date = input.Date;
            BlockReward = input.BlockReward;
            LastBlock = input.LastBlock;
            Difficulty = input.Difficulty;
            Nethash = input.Nethash;
            ExRate = input.ExRate;
            ExchangeRateVol = input.ExchangeRateVol;
            MarketCap = input.MarketCap;
            PoolFee = input.PoolFee;
            DailyEmission = input.DailyEmission;
            Coin = input.Coin.Name;
        }
    }
}
