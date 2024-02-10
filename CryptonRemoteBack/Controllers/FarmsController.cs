using CryptonRemoteBack.Domain;
using CryptonRemoteBack.Domain.CoinsDatabase;
using CryptonRemoteBack.Extensions;
using CryptonRemoteBack.Infrastructure;
using CryptonRemoteBack.Model;
using CryptonRemoteBack.Model.Models;
using CryptonRemoteBack.Model.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace CryptonRemoteBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FarmsController : ControllerBase
    {
        private string UserId => User.GetUserId();
        private readonly IConfiguration _configuration;

        private readonly string? python_path;
        private readonly string? predict_py_path;

        public FarmsController(IConfiguration configuration)
        {
            _configuration = configuration;

            python_path = configuration["PythonPath"];
            predict_py_path = configuration["PredictPyPath"];

            if (string.IsNullOrWhiteSpace(python_path))
                python_path = "C:\\Users\\kseny\\AppData\\Local\\Programs\\Python\\Python311\\python.exe";
            if (string.IsNullOrWhiteSpace(predict_py_path))
                predict_py_path = "D:\\osiris\\Crypton\\Crypton_Python\\MLCrypton\\predict.py";
        }


        /// <summary>
        /// Создание фермы
        /// </summary>
        /// <param name="input">Входные данные</param>
        [HttpPost("/api/farms/add")]
        [Authorize]
        public async Task<ActionResult> AddFarm(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromBody] FarmAddModel input,
            CancellationToken ct)
        {
            Farm farm = new()
            {
                User = await db.Users.FirstAsync(x => x.Id == UserId.ToString(), ct),
                Name = input.Name,
                LocalSystemID = GetRandomString(7)
            };

            await db.Farms.AddAsync(farm, ct);
            await db.SaveChangesAsync(ct);
            return Ok(farm.LocalSystemID);
        }

        private static string GetRandomString(int length)
        {
            Random r = new();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            return new string(
                Enumerable.Repeat(chars, length).Select(s => s[r.Next(s.Length)]).ToArray());
        }


        /// <summary>
        /// Удаление фермы
        /// </summary>
        /// <param name="farmId">Идентификатор фермы</param>
        [HttpDelete("/api/farms/delete/{farmId:int}")]
        [Authorize]
        public async Task<ActionResult> DeleteFarm(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] int farmId,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == farmId, ct);

            if (farm == null || farm.User.Id != UserId)
            {
                return NotFound($"Farm {farmId} not found for current user");
            }

            db.Farms.Remove(farm);
            await db.SaveChangesAsync(ct);
            return Ok($"Farm {farmId} was deleted");
        }


        /// <summary>
        /// Получение списка ферм пользователя
        /// </summary>
        [HttpGet("/api/farms/all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FarmView>>> GetAllFarms(
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            List<Farm> farms = await db.Farms
                .Include(x => x.User)
                .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Miner)
                .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Wallet)
                .Where(x => x.User.Id == UserId).ToListAsync(ct);

            return Ok(farms.Select(x => new FarmView(x)));
        }


        /// <summary>
        /// Получение информации о ферме
        /// </summary>
        /// <param name="farmId">Идентификатор фермы</param>
        [HttpGet("/api/farms/{farmId:int}")]
        [Authorize]
        public async Task<ActionResult<FarmView>> GetFarm(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] int farmId,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .Include(x => x.User)
                .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Miner)
                .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Wallet)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == farmId, ct);

            if (farm == null)
            {
                return NotFound($"Farm {farmId} not found for current user");
            }

            return Ok(new FarmView(farm));
        }


        /// <summary>
        /// Выставление параметров разгона фермы
        /// </summary>
        /// <param name="farmId">Идентификатор фермы</param>
        [HttpPost("/api/farms/{farmId:int}/set_overclocking")]
        [Authorize]
        public async Task<IActionResult> FarmSetOverclocking(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] int farmId,
            [FromBody] FarmOverclockingModel input,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == farmId, ct);

            if (farm == null)
            {
                return NotFound($"Farm {farmId} not found for current user");
            }

            if (await FarmsHelpers.Overclocking(farm.LocalSystemAddress, input))
            {
                return Ok("Success");
            }

            return BadRequest("Failed");
        }


        /// <summary>
        /// Добавление параметров разгона фермы в бд
        /// </summary>
        /// <param name="input">Входные параметры</param>
        [HttpPost("/api/farms/add_overclocking")]
        [Authorize]
        public async Task<IActionResult> FarmSelectOverclocking(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromBody] FarmOverclockingModel input,
            CancellationToken ct)
        {
            OverclockingParams parameters = new()
            {
                CoreFrequency = input.СoreFrequency,
                MemoryFrequency = input.MemoryFrequency,
                CoolerSpeed = input.CoolerSpeed,
                Consumption = input.Consumption
            };
            _ = await db.OverclockingParams.AddAsync(parameters, ct);
            _ = await db.SaveChangesAsync(ct);
            return Ok(parameters.Id);
        }


        /// <summary>
        /// Выбор параметров разгона фермы
        /// </summary>
        /// <param name="input">Входные параметры</param>
        [HttpPost("/api/farms/select_overclocking")]
        [Authorize]
        public async Task<IActionResult> FarmSelectOverclocking(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromBody] SelectOverclockingModel input,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == input.FarmId, ct);

            if (farm == null)
            {
                return NotFound($"Farm {input.FarmId} not found for current user");
            }

            OverclockingParams parameters = await db.OverclockingParams
                .FirstAsync(x => x.Id == input.OverclockingParamsId, ct);
            parameters.Counter += 1;
            _ = await db.SaveChangesAsync(ct);

            if (await FarmsHelpers.Overclocking(farm.LocalSystemAddress,
                new FarmOverclockingModel()
                {
                    СoreFrequency = parameters.CoreFrequency,
                    MemoryFrequency = parameters.MemoryFrequency,
                    CoolerSpeed = parameters.CoolerSpeed,
                    Consumption = parameters.Consumption,
                }))
            {
                return Ok("Success");
            }

            return BadRequest("Failed");
        }


        /// <summary>
        /// Просмотр доступных параметров разгона фермы
        /// </summary>
        [HttpGet("/api/farms/get_overclocking")]
        [Authorize]
        public async Task<IActionResult> GetOverclocking(
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            List<OverclockingParams> parameters = await db.OverclockingParams.ToListAsync(ct);
            return Ok(parameters);
        }


        /// <summary>
        /// Просмотр наиболее часто используемых параметров разгона фермы
        /// </summary>
        [HttpGet("/api/farms/get_best_overclocking")]
        [Authorize]
        public async Task<IActionResult> GetBestOverclocking(
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            List<OverclockingParams> parameters = await db.OverclockingParams.ToListAsync(ct);
            return Ok(parameters.OrderByDescending(x => x.Counter).Take(3));
        }


        /// <summary>
        /// Заупск фермы
        /// </summary>
        /// <param name="farmId">Идентификатор фермы</param>
        [HttpGet("/api/farms/{farmId:int}/start")]
        [Authorize]
        public async Task<IActionResult> StartFarm(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] int farmId,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .Include(x => x.User)
                .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Miner)
                .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Wallet)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == farmId, ct);

            if (farm == null)
            {
                return NotFound($"Farm {farmId} not found for current user");
            }

            if (farm.ActiveFlightSheet == null)
            {
                return BadRequest($"Farm {farmId} has no active flight sheet");
            }

            if (await FarmsHelpers.StartFarm(farm.LocalSystemAddress,
                                             farm.ActiveFlightSheet.Miner.ContainerName,
                                             farm.ActiveFlightSheet.PoolAddress,
                                             farm.ActiveFlightSheet.Wallet.Address,
                                             farm.ActiveFlightSheet.ExtendedConfig))
            {
                return Ok("Success");
            }

            return BadRequest("Failed");
        }


        /// <summary>
        /// Запуск майнинга в автоматическом режиме
        /// </summary>
        /// <param name="farmId">Идентификатор фермы</param>
        [HttpGet("/api/farms/{farmId:int}/start_auto_mode")]
        [Authorize]
        public async Task<IActionResult> StartFarmAutoMode(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromServices] DataParserDbContext db_data,
            [FromRoute] int farmId,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .Include(x => x.User)
                .Include(x => x.ActiveFlightSheet)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == farmId, ct);

            if (farm == null)
            {
                return NotFound($"Farm {farmId} not found for current user");
            }

            List<FsHolder> sheets = await db.FlightSheets
                .Include(x => x.User)
                .Include(x => x.Wallet).ThenInclude(x => x.Currency)
                .Where(x => x.User.Id == UserId)
                .AsNoTracking().Select(x => new FsHolder(x)).ToListAsync(ct);

            for (int i = 0; i < sheets.Count; i++)
            {
                double hash;
                if (sheets[i].Hashrate >= 100.0 && sheets[i].Hashrate <= 9950.0)
                {
                    for (hash = 100.0; hash <= 9950.0; hash += 50.0)
                        if (Math.Abs(hash - sheets[i].Hashrate) <= 25.0) break;
                }
                else hash = sheets[i].Hashrate < 100.0 ? 100.0 : 9950.0;

                List<Monitoring> mons = await db_data.Monitorings
                    .Include(x => x.Coin)
                    .Where(x => x.Coin.Name == sheets[i].CurrencyName && x.Hashrate == hash)
                    .OrderByDescending(x => x.Date).Take(2)
                    .AsNoTracking().ToListAsync(ct);

                Monitoring lastMon = mons.MaxBy(x => x.Date)!;
                Monitoring firstMon = mons.MinBy(x => x.Date)!;
                ParamsDiff diff = new(firstMon, lastMon);

                string request = $"{predict_py_path} {sheets[i].CurrencyName} " +
                            $"{lastMon.BlockReward + diff.BlockReward} " +
                            $"{lastMon.LastBlock + diff.LastBlock} " +
                            $"{lastMon.Difficulty + diff.Difficulty} " +
                            $"{lastMon.Nethash + diff.Nethash} " +
                            $"{lastMon.ExRate + diff.ExRate} " +
                            $"{lastMon.ExchangeRateVol + diff.ExchangeRateVol} " +
                            $"{lastMon.MarketCap + diff.MarketCap} " +
                            $"{lastMon.PoolFee + diff.PoolFee} " +
                            $"{sheets[i].Hashrate}";

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
                sheets[i].Prediction = double.TryParse(script_result.Trim().Replace("[", "").Replace("]", ""), out double res)
                    ? res : 0.0;
            }

            FsHolder? optimal = sheets.MaxBy(x => x.Prediction);
            FlightSheet optimalFs = await db.FlightSheets
                .Include(x => x.Miner)
                .Include(x => x.Wallet)
                .FirstAsync(x => x.Id == optimal!.FsId, ct);

            farm.ActiveFlightSheet = optimalFs;
            await db.SaveChangesAsync(ct);
            return Ok($"Success, launching coin {optimal!.CurrencyName}");

            try
            {
                if (await FarmsHelpers.StartFarm(farm.LocalSystemAddress,
                                                 optimalFs.Miner.ContainerName,
                                                 optimalFs.PoolAddress,
                                                 optimalFs.Wallet.Address,
                                                 optimalFs.ExtendedConfig))
                {
                    return Ok($"Success, launching coin {optimal!.CurrencyName} (prediction {optimal.Prediction})");
                }

                return BadRequest($"Failure, launching coin {optimal!.CurrencyName} (prediction {optimal.Prediction})");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message} {ex.StackTrace}");
            }
        }


        /// <summary>
        /// Остановка фермы
        /// </summary>
        /// <param name="farmId">Идентификатор фермы</param>
        [HttpGet("/api/farms/{farmId:int}/stop")]
        [Authorize]
        public async Task<IActionResult> StopFarm(
        [FromServices] CryptonRemoteBackDbContext db,
        [FromRoute] int farmId,
        CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .Include(x => x.User)
                .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Miner)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == farmId, ct);

            if (farm == null)
            {
                return NotFound($"Farm {farmId} not found for current user");
            }

            if (farm.ActiveFlightSheet == null)
            {
                return BadRequest($"Farm {farmId} has no active flight sheet");
            }

            if (await FarmsHelpers.StopFarm(farm.LocalSystemAddress,
                                            farm.ActiveFlightSheet.Miner.ContainerName))
            {
                return Ok("Success");
            }

            return BadRequest("Failed");
        }


        /// <summary>
        /// Перезапуск фермы
        /// </summary>
        /// <param name="farmId">Идентификатор фермы</param>
        [HttpGet("/api/farms/{farmId:int}/restart")]
        [Authorize]
        public async Task<IActionResult> RestartFarm(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] int farmId,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .Include(x => x.User)
                .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Miner)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == farmId, ct);

            if (farm == null)
            {
                return NotFound($"Farm {farmId} not found for current user");
            }

            if (farm.ActiveFlightSheet == null)
            {
                return BadRequest($"Farm {farmId} has no active flight sheet");
            }

            if (await FarmsHelpers.RestartFarm(farm.LocalSystemAddress,
                                               farm.ActiveFlightSheet.Miner.ContainerName))
            {
                return Ok("Success");
            }

            return BadRequest("Failed");
        }


        //[HttpGet("/api/farms/{farmId:int}/get_monitorings")]
        //[Authorize]
        //public async Task<ActionResult<List<MinerMonitoringRecord>?>> GetFarmMonitorings(
        //    [FromServices] CryptonRemoteBackDbContext db,
        //    [FromRoute] int farmId,
        //    CancellationToken ct)
        //{
        //    Farm? farm = await db.Farms.Include(x => x.User)
        //        .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == farmId, ct);

        //    if (farm == null)
        //    {
        //        return NotFound($"Farm {farmId} not found for current user");
        //    }

        //    List<MinerMonitoringRecord>? result = await FarmsHelpers.GetMonitorings(farm.LocalSystemAddress);
        //    return result;
        //}


        //[HttpGet("/api/farms/get_all_farms_monitorings")]
        //[Authorize]
        //public async Task<ActionResult<List<FarmMonitoringView>>> GetAllFarmsMonitorings(
        //    [FromServices] CryptonRemoteBackDbContext db,
        //    CancellationToken ct)
        //{
        //    List<Farm> farms = await db.Farms
        //        .Include(x => x.User)
        //        .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Miner)
        //        .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Wallet)
        //        .Where(x => x.User.Id == UserId).AsNoTracking().ToListAsync(ct);

        //    if (farms == null || farms.Count == 0)
        //    {
        //        return NotFound($"Farms not found for current user");
        //    }

        //    List<FarmMonitoringView> result = new();
        //    foreach (Farm farm in farms)
        //    {
        //        result.Add(new(farm, await FarmsHelpers.GetMonitorings(farm.LocalSystemAddress)));
        //    }

        //    return result;
        //}


        /// <summary>
        /// Смена полётного листа фермы
        /// </summary>
        /// <param name="flightSheetId">Идентификатор полётного листа</param>
        /// <param name="farmId">Идентификатор фермы</param>
        [HttpPatch("/api/farms/{farmId:int}/switch_flight_sheet")]
        [Authorize]
        public async Task<ActionResult> SwitchFlightSheet(
            [FromBody] int? flightSheetId,
            [FromRoute] int farmId,
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .Include(x => x.User)
                .Include(x => x.ActiveFlightSheet)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == farmId, ct);

            if (farm == null)
            {
                return NotFound($"Farm {farmId} not found for current user");
            }

            FlightSheet? fs = await db.FlightSheets
                .FirstOrDefaultAsync(x => x.Id == flightSheetId, ct);

            if (flightSheetId != null && fs == null)
            {
                return BadRequest($"FlightSheet {flightSheetId} not found");
            }

            farm.ActiveFlightSheet = fs;
            await db.SaveChangesAsync(ct);
            return Ok(farm.Id);
        }


        /// <summary>
        /// Редистрация фермы в системе (вызывается локальной системой)
        /// </summary>
        /// <param name="input">Входные данные</param>
        [HttpPost("/api/farms/register")]
        public async Task<ActionResult> RegisterFarm(
            [FromBody] FarmRegisterModel input,
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .FirstOrDefaultAsync(x => x.LocalSystemID == input.LocalSystemID, ct);

            if (farm == null)
            {
                return NotFound($"Farm {input.LocalSystemID} not found");
            }

            if (IPAddress.TryParse(input.LocalSystemAddress, out _))
            {
                farm.LocalSystemAddress = input.LocalSystemAddress;
                farm.SystemInfo = JsonConvert.SerializeObject(input.SystemInfo);
                await db.SaveChangesAsync(ct);
                return Ok(farm.Id);
            }
            else
            {
                return BadRequest("Incorrect IP address");
            }
        }


        /// <summary>
        /// Измененеие данных о локальной системе (вызывается локальной системой)
        /// </summary>
        /// <param name="input">Входные данные</param>
        [HttpPost("/api/farms/set_info")]
        public async Task<ActionResult> SetInfo(
            [FromForm] FarmInfoModel input,
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .FirstOrDefaultAsync(x => x.LocalSystemID == input.LocalSystemID, ct);

            if (farm == null)
            {
                return NotFound($"Farm {input.LocalSystemID} not found");
            }

            farm.SystemInfo = input.SystemInfo;
            await db.SaveChangesAsync(ct);
            return Ok(farm.Id);
        }


        /// <summary>
        /// Получение данных мониторинга ферм пользователя
        /// </summary>
        /// <param name="token">Токен авторизации</param>
        [HttpGet("/api/farms/stats")]
        public async Task GetStats([FromServices] CryptonRemoteBackDbContext db,
                                   [FromQuery] string token,
                                   CancellationToken ct)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                string? id;
                try
                {
                    TokenValidationParameters? tokenValParams = new()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(_configuration["JWT:Secret"]))
                    };
                    JwtSecurityTokenHandler handler = new();
                    handler.ValidateToken(token, tokenValParams, out SecurityToken securityToken);
                    JwtSecurityToken? jwt_token = securityToken as JwtSecurityToken;
                    id = jwt_token?.Claims.First(claim => claim.Type == "UserId").Value;
                    if (id == null) throw new Exception();
                }
                catch
                {
                    byte[] message = Encoding.UTF8.GetBytes("invalid token");
                    await webSocket.SendAsync(new ArraySegment<byte>(message, 0, message.Length),
                                              WebSocketMessageType.Text,
                                              true,
                                              CancellationToken.None);
                    HttpContext.Response.StatusCode = 401;
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                                               "invalid token",
                                               CancellationToken.None);
                    return;
                }

                byte[] buffer = new byte[1024 * 4];
                var task = webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                while (!task.IsCompleted)
                {
                    List<Farm> farms = await db.Farms
                        .Include(x => x.User)
                        .Include(x => x.ActiveFlightSheet)
                        .Where(x => x.User.Id == id).AsNoTracking().ToListAsync(ct);

                    if (farms == null || farms.Count == 0)
                    {
                        byte[] message = Encoding.UTF8.GetBytes("no farms");
                        await webSocket.SendAsync(new ArraySegment<byte>(message, 0, message.Length),
                                                  WebSocketMessageType.Text,
                                                  true,
                                                  CancellationToken.None);
                    }

                    await FarmsHelpers.GetStats(webSocket,
                        farms.Select(x => (x.Id,
                                           x.ActiveFlightSheet != null ? x.ActiveFlightSheet.Id : 0,
                                           x.LocalSystemAddress)).ToList());
                }
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                                           "ended",
                                           CancellationToken.None);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }

    internal class FsHolder
    {
        internal int FsId { get; set; }
        internal double Hashrate { get; set; }
        internal string CurrencyName { get; set; }
        internal double Prediction { get; set; }

        public FsHolder(FlightSheet input)
        {
            FsId = input.Id;
            Hashrate = input.Hashrate;
            CurrencyName = input.Wallet.Currency.Name;
            Prediction = 0.0;
        }
    }
}