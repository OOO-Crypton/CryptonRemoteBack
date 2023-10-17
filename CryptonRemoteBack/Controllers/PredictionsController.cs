using CryptonRemoteBack.Domain;
using CryptonRemoteBack.Domain.CoinsDatabase;
using CryptonRemoteBack.Infrastructure;
using CryptonRemoteBack.Model.Models;
using CryptonRemoteBack.Model.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CryptonRemoteBack.Controllers
{
    [ApiController]
    public class PredictionsController : ControllerBase
    {
        private readonly string? python_path;
        private readonly string? predict_py_path;

        public PredictionsController(IConfiguration config)
        {
            python_path = config["PythonPath"];
            predict_py_path = config["PredictPyPath"];

            if (string.IsNullOrWhiteSpace(python_path))
                python_path = "C:\\Users\\kseny\\AppData\\Local\\Programs\\Python\\Python311\\python.exe";
            if (string.IsNullOrWhiteSpace(predict_py_path))
                predict_py_path = "D:\\osiris\\Crypton\\Crypton_Python\\MLCrypton\\predict.py";
        }


        [HttpGet("/predictions/get_prediction")]
        [Authorize]
        public async Task<ActionResult<List<PredictionView>>> GetPrediction(
            [FromQuery] PredictionModel input,
            [FromServices] CryptonRemoteBackDbContext db,
            [FromServices] DataParserDbContext db_data,
            CancellationToken ct)
        {
            try
            {
                Currency? currency = await db.Currencies
                    .FirstOrDefaultAsync(x => x.Id == input.CurrencyId, ct);
                if (currency == null)
                {
                    return BadRequest($"Currency with id = {input.CurrencyId} not found");
                }

                List<PredictionView> predictions = new();

                double hash;
                if (input.HashRate >= 100.0 && input.HashRate <= 9950.0)
                {
                    for (hash = 100.0; hash <= 9950.0; hash += 50.0)
                        if (Math.Abs(hash - input.HashRate) <= 25.0) break;
                }
                else hash = input.HashRate < 100.0 ? 100.0 : 9950.0;

                List<Monitoring> previousWeekMons = await db_data.Monitorings
                    .Include(x => x.Coin)
                    .Where(x => x.Coin.Name == currency.Name
                                && x.Date >= DateTime.UtcNow.AddDays(-7)
                                && x.Hashrate == hash)
                    .AsNoTracking().ToListAsync(ct);

                if (previousWeekMons == null || !previousWeekMons.Any())
                {
                    return Ok(predictions);
                }

                Monitoring lastMon = previousWeekMons.MaxBy(x => x.Date)!;
                Monitoring firstMon = previousWeekMons.MinBy(x => x.Date)!;

                Dictionary<int, ParamsDiff?> diffs = new();

                for (int i = -6; i < 0; i++)
                {
                    List<Monitoring> prevMon = previousWeekMons
                        .Where(x => x.Date.Date >= DateTime.UtcNow.AddDays(i).Date).ToList();

                    diffs.Add(i, prevMon.Any() ? new(firstMon, prevMon.MinBy(x => x.Date)!) : null);
                }
                diffs.Add(0, new(firstMon, lastMon));

                int days = 1;
                for (int i = -6; i <= 0; i++)
                {
                    if (diffs[i] == null)
                    {
                        predictions.Add(new(DateTime.Now.Date.AddDays(days), -1.0, ""));
                        continue;
                    }

                    string request = $"{predict_py_path} {currency.Name} " +
                            $"{lastMon.BlockReward + diffs[i].BlockReward} " +
                            $"{lastMon.LastBlock + diffs[i].LastBlock} " +
                            $"{lastMon.Difficulty + diffs[i].Difficulty} " +
                            $"{lastMon.Nethash + diffs[i].Nethash} " +
                            $"{lastMon.ExRate + diffs[i].ExRate} " +
                            $"{lastMon.ExchangeRateVol + diffs[i].ExchangeRateVol} " +
                            $"{lastMon.MarketCap + diffs[i].MarketCap} " +
                            $"{lastMon.PoolFee + diffs[i].PoolFee} " +
                            $"{input.HashRate}";
                    string script_result = "";
                    ProcessStartInfo start = new()
                    {
                        FileName = python_path,
                        Arguments = request,
                        UseShellExecute = false,
                        RedirectStandardOutput = true
                    };
                    using (Process process = Process.Start(start)!)
                    {
                        using StreamReader reader = process.StandardOutput;
                        script_result = reader.ReadToEnd();
                    }
                    if (double.TryParse(script_result.Trim().Replace("[", "").Replace("]", ""), out double res))
                        predictions.Add(new(DateTime.Now.Date.AddDays(days), res, request));
                    else predictions.Add(new(DateTime.Now.Date.AddDays(days), double.NaN, request));

                    days++;
                }

                return Ok(predictions);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.StackTrace}");
            }
        }


        [HttpGet("/predictions/get_monitorings/{coinId:int}/{hashrate:double}")]
        [Authorize]
        public async Task<ActionResult<List<MonitoringView>>> GetMonitorings(
            [FromRoute] int coinId,
            [FromRoute] double hashrate,
            [FromServices] CryptonRemoteBackDbContext db,
            [FromServices] DataParserDbContext db_data,
            CancellationToken ct)
        {
            try
            {
                Currency? currency = await db.Currencies
                    .FirstOrDefaultAsync(x => x.Id == coinId, ct);
                if (currency == null)
                {
                    return BadRequest($"Currency with id = {coinId} not found");
                }

                List<Monitoring> mons = await db_data.Monitorings
                    .Include(x => x.Coin)
                    .Where(x => x.Coin.Name == currency.Name && (hashrate <= 0.0 || x.Hashrate == hashrate))
                    .AsNoTracking().ToListAsync(ct);

                return new List<MonitoringView>(mons.Select(x => new MonitoringView(x)));
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}\n{ex.StackTrace}");
            }
        }
    }

    public class ParamsDiff
    {
        public double BlockReward { get; set; }
        public double LastBlock { get; set; }
        public double Difficulty { get; set; }
        public double Nethash { get; set; }
        public double ExRate { get; set; }
        public double ExchangeRateVol { get; set; }
        public double MarketCap { get; set; }
        public double PoolFee { get; set; }

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
        }
    }
}
