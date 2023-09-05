using CryptonRemoteBack.Domain;
using CryptonRemoteBack.Domain.CoinsDatabase;
using CryptonRemoteBack.Infrastructure;
using CryptonRemoteBack.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CryptonRemoteBack.Controllers
{
    [ApiController]
    public class PredictionsController : ControllerBase
    {
        [HttpGet("/predictions/get_prediction")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<double>>> GetPrediction(
            [FromQuery] PredictionModel input,
            [FromServices] CryptonRemoteBackDbContext db,
            [FromServices] DataParserDbContext db_data,
            CancellationToken ct)
        {
            Currency? currency = await db.Currencies
                .FirstOrDefaultAsync(x => x.Id == input.CurrencyId, ct);

            if (currency == null)
            {
                return BadRequest($"Currency {input.CurrencyId} not found");
            }

            List<Monitoring> previousWeekMons = await db_data.Monitorings
                .Include(x => x.Coin)
                .Where(x => x.Coin.Name == currency.Name && x.Date >= DateTime.UtcNow.AddDays(-7))
                .AsNoTracking()
                .ToListAsync(ct);

            Monitoring lastMon = previousWeekMons.MaxBy(x => x.Date)!;
            Monitoring firstMon = previousWeekMons.MinBy(x => x.Date)!;

            Dictionary<int, ParamsDiff> diffs = new();
            
            for (int i = -6; i < 0; i++)
            {
                diffs.Add(i, new(firstMon,
                    previousWeekMons
                        .Where(x => x.Date >= DateTime.UtcNow.AddDays(i))
                        .MinBy(x => x.Date)!));
            }
            diffs.Add(0, new(firstMon, lastMon));

            List<double> predictions = new();
            for (int i = -6; i <= 0; i++)
            {
                // TODO вызов прогноза, передача этих параметров
                // (куда хэш)
                // ещё надо путь до предсказывателя в конфиг засунуть
                Process.Start($"{currency.Name} " +
                    $"{lastMon.BlockReward + diffs[i].BlockReward}" +
                    $"{lastMon.LastBlock + diffs[i].LastBlock}" +
                    $"{lastMon.Difficulty + diffs[i].Difficulty}" +
                    $"{lastMon.Nethash + diffs[i].Nethash}" +
                    $"{lastMon.ExRate + diffs[i].ExRate}" +
                    $"{lastMon.ExchangeRateVol + diffs[i].ExchangeRateVol}" +
                    $"{lastMon.MarketCap + diffs[i].MarketCap}" +
                    $"{lastMon.PoolFee + diffs[i].PoolFee}" +
                    $"{lastMon.DailyEmission + diffs[i].DailyEmission}");
                // TODO чтение вывода прогноза
            }


            return Ok(predictions);
        }
    }

    internal class ParamsDiff
    {
        public double BlockReward { get; set; }
        public double LastBlock { get; set; }
        public double Difficulty { get; set; }
        public double Nethash { get; set; }
        public double ExRate { get; set; }
        public double ExchangeRateVol { get; set; }
        public double MarketCap { get; set; }
        public double PoolFee { get; set; }
        public double DailyEmission { get; set; }

        public ParamsDiff(Monitoring first, Monitoring second)
        {
            BlockReward = second.BlockReward - first.BlockReward;
            LastBlock = second.LastBlock - first.LastBlock;
            Difficulty = second.Difficulty - first.Difficulty;
            Nethash = second.Nethash - first.Nethash;
            ExRate = second.ExRate - first.ExRate;
            ExchangeRateVol = second.ExchangeRateVol - first.ExchangeRateVol;
            MarketCap = second.MarketCap - first.MarketCap;
            PoolFee = second.PoolFee - first.PoolFee;
            DailyEmission = second.DailyEmission - first.DailyEmission;
        }
    }
}
