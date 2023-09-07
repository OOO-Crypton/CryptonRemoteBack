﻿using CryptonRemoteBack.Domain;
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
        private readonly string python_path;
        private readonly string predict_py_path;

        public PredictionsController(IConfiguration config)
        {
            python_path = config["PythonPath"]
                ?? "C:\\Users\\kseny\\AppData\\Local\\Programs\\Python\\Python311\\python.exe";
            predict_py_path = config["PredictPyPath"]
                ?? "D:\\osiris\\Crypton\\Crypton_Python\\MLCrypton\\predict.py";
        }


        [HttpGet("/predictions/get_prediction")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<double>>> GetPrediction(
            [FromQuery] PredictionModel input,
            [FromServices] CryptonRemoteBackDbContext db,
            //[FromServices] DataParserDbContext db_data,
            CancellationToken ct)
        {
            Currency? currency = await db.Currencies
                .FirstOrDefaultAsync(x => x.Id == input.CurrencyId, ct);

            if (currency == null)
            {
                return BadRequest($"Currency with id = {input.CurrencyId} not found");
            }

            List<double> predictions = new();
            List<Monitoring> previousWeekMons = new();
            //List<Monitoring> previousWeekMons = await db_data.Monitorings
            //    .Include(x => x.Coin)
            //    .Where(x => x.Coin.Name == currency.Name && x.Date >= DateTime.UtcNow.AddDays(-7))
            //    .AsNoTracking().ToListAsync(ct);

            if (previousWeekMons == null || !previousWeekMons.Any())
            {
                //если недостаточно предыдущих наблюдений - возвращаем нули
                predictions.AddRange(new List<double>() { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
                return predictions;
            }

            Monitoring lastMon = previousWeekMons.MaxBy(x => x.Date)!;
            Monitoring firstMon = previousWeekMons.MinBy(x => x.Date)!;

            Dictionary<int, ParamsDiff> diffs = new();

            for (int i = -6; i < 0; i++)
            {
                diffs.Add(i, new(firstMon, previousWeekMons
                    .Where(x => x.Date >= DateTime.UtcNow.AddDays(i))
                    .MinBy(x => x.Date)!));
            }
            diffs.Add(0, new(firstMon, lastMon));
            for (int i = -6; i <= 0; i++)
            {
                // TODO куда хэш
                string result = "";
                ProcessStartInfo start = new()
                {
                    FileName = python_path,
                    Arguments = $"{predict_py_path} {currency.Name} " +
                        $"{lastMon.BlockReward + diffs[i].BlockReward} " +
                        $"{lastMon.LastBlock + diffs[i].LastBlock} " +
                        $"{lastMon.Difficulty + diffs[i].Difficulty} " +
                        $"{lastMon.Nethash + diffs[i].Nethash} " +
                        $"{lastMon.ExRate + diffs[i].ExRate} " +
                        $"{lastMon.ExchangeRateVol + diffs[i].ExchangeRateVol} " +
                        $"{lastMon.MarketCap + diffs[i].MarketCap} " +
                        $"{lastMon.PoolFee + diffs[i].PoolFee} " +
                        $"{lastMon.DailyEmission + diffs[i].DailyEmission}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                };
                using (Process process = Process.Start(start)!)
                {
                    using StreamReader reader = process.StandardOutput;
                    result = reader.ReadToEnd();
                }
                predictions.Add(double.Parse(result.Trim().Replace("[", "").Replace("]", "")));
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