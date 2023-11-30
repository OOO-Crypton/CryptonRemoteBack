using CryptonRemoteBack.Domain;
using CryptonRemoteBack.Extensions;
using CryptonRemoteBack.Infrastructure;
using CryptonRemoteBack.Model.Models;
using CryptonRemoteBack.Model.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptonRemoteBack.Controllers
{
    [ApiController]
    public class FlightSheetsController : ControllerBase
    {
        private string UserId => User.GetUserId();

        public FlightSheetsController() { }


        /// <summary>
        /// Создание полётного листа
        /// </summary>
        /// <param name="input">Входные данные</param>
        [HttpPost("/api/flight_sheets/add")]
        [Authorize]
        public async Task<ActionResult> AddFlightSheet(
            [FromForm] FlightSheetModel input,
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(input.Name)
                || input.MinerId == null
                || input.WalletId == null)
            {
                return BadRequest("Fields Name, MinerId, WalletId or PoolId is empty");
            }

            Miner? miner = await db.Miners.FirstOrDefaultAsync(x => x.Id == input.MinerId, ct);
            Wallet? wallet = await db.Wallets.FirstOrDefaultAsync(x => x.Id == input.WalletId, ct);

            if (miner == null || wallet == null)
            {
                return BadRequest($"Miner/wallet not found");
            }

            FlightSheet flightSheet = new()
            {
                Name = input.Name,
                ExtendedConfig = input.ExtendedConfig ?? "",
                User = await db.Users.FirstAsync(x => x.Id == UserId.ToString(), ct),
                Miner = miner,
                Wallet = wallet,
                PoolAddress = input.PoolAddress
            };

            await db.FlightSheets.AddAsync(flightSheet, ct);
            await db.SaveChangesAsync(ct);
            return Ok(flightSheet.Id);
        }


        /// <summary>
        /// Удаление полётного листа
        /// </summary>
        /// <param name="flightSheetId">Идентификатор листа</param>
        [HttpDelete("/api/flight_sheets/delete/{flightSheetId:int}")]
        [Authorize]
        public async Task<ActionResult> DeleteFlightSheet(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] int flightSheetId,
            CancellationToken ct)
        {
            FlightSheet? flightSheet = await db.FlightSheets
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == flightSheetId, ct);

            if (flightSheet == null || flightSheet.User.Id != UserId)
            {
                return NotFound($"FlightSheet {flightSheetId} not found for current user");
            }

            Farm? farm = await db.Farms
                .Include(x => x.ActiveFlightSheet)
                .FirstOrDefaultAsync(x => x.ActiveFlightSheet != null
                                          && x.ActiveFlightSheet.Id == flightSheet.Id, ct);
            if (farm != null)
            {
                farm.ActiveFlightSheet = null;
                await db.SaveChangesAsync(ct);
            }

            db.FlightSheets.Remove(flightSheet);
            await db.SaveChangesAsync(ct);
            return Ok($"FlightSheet {flightSheet.Id} was deleted");
        }


        /// <summary>
        /// Получение всех полётных листов пользователя
        /// </summary>
        [HttpGet("/api/flight_sheets/all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FlightSheetView>>> GetAllFlightSheets(
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            List<FlightSheet> flightSheets = await db.FlightSheets
                .Include(x => x.User)
                .Include(x => x.Miner)
                .Include(x => x.Wallet).ThenInclude(x => x.Currency)
                .Where(x => x.User.Id == UserId).ToListAsync(ct);

            List<Farm> farms = await db.Farms
                .Include(x => x.User)
                .Include(x => x.ActiveFlightSheet)
                .Where(x => x.User.Id == UserId).ToListAsync(ct);

            return Ok(flightSheets.Select(x =>
                new FlightSheetView(x, farms.Any(x => x.ActiveFlightSheet?.Id == x.Id))));
        }


        /// <summary>
        /// Получение информации о полётном листе пользователя
        /// </summary>
        /// <param name="flightSheetId">Идентификатор листа</param>
        [HttpGet("/api/flight_sheets/{flightSheetId:int}")]
        [Authorize]
        public async Task<ActionResult<FlightSheetView>> GetFlightSheet(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] int flightSheetId,
            CancellationToken ct)
        {
            FlightSheet? flightSheet = await db.FlightSheets
                .Include(x => x.User)
                .Include(x => x.Miner)
                .Include(x => x.Wallet).ThenInclude(x => x.Currency)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == flightSheetId, ct);

            if (flightSheet == null)
            {
                return NotFound($"FlightSheet {flightSheetId} not found for current user");
            }

            Farm? farm = await db.Farms
                .Include(x => x.User)
                .Include(x => x.ActiveFlightSheet)
                .FirstOrDefaultAsync(x => x.User.Id == UserId
                    && x.ActiveFlightSheet != null
                    && x.ActiveFlightSheet.Id == flightSheet.Id, ct);

            return Ok(new FlightSheetView(flightSheet, farm != null));
        }


        /// <summary>
        /// Поулчение словаря майнеров
        /// </summary>
        [HttpGet("/api/flight_sheets/miners_list")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MinerView>>> GetMiners(
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            List<MinerView> miners = await db.Miners
                .Select(x => new MinerView(x)).ToListAsync(ct);
            return Ok(miners);
        }


        /// <summary>
        /// Получение информации о майнере
        /// </summary>
        /// <param name="minerId">Идентификатор майнера</param>
        [HttpGet("/api/flight_sheets/miners_list/{minerId:int}")]
        [Authorize]
        public async Task<ActionResult<MinerView>> GetMinerInfo(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] int minerId,
            CancellationToken ct)
        {
            Miner? miner = await db.Miners.FirstOrDefaultAsync(x => x.Id == minerId, ct);
            if (miner == null)
            {
                return NotFound($"Miner {minerId} not found");
            }
            return Ok(new MinerView(miner));
        }


        /// <summary>
        /// Редактирование полётного листа
        /// </summary>
        /// <param name="input">Входная информация</param>
        /// <param name="flightSheetId">Идентификатор листа</param>
        [HttpPut("/api/flight_sheets/{flightSheetId:int}/edit")]
        [Authorize]
        public async Task<ActionResult> EditFlightSheet(
            [FromForm] FlightSheetModel input,
            [FromRoute] int flightSheetId,
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            FlightSheet? flightSheet = await db.FlightSheets
                .Include(x => x.User)
                .Include(x => x.Miner)
                .Include(x => x.Wallet).ThenInclude(x => x.Currency)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == flightSheetId, ct);

            if (flightSheet == null)
            {
                return NotFound($"FlightSheet {flightSheetId} not found for current user");
            }

            Miner? miner = await db.Miners.FirstOrDefaultAsync(x => x.Id == input.MinerId, ct);
            Wallet? wallet = await db.Wallets.FirstOrDefaultAsync(x => x.Id == input.WalletId, ct);

            if (input.MinerId != null && miner != null)
            {
                flightSheet.Miner = miner;
            }

            if (input.WalletId != null && wallet != null)
            {
                flightSheet.Wallet = wallet;
            }

            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                flightSheet.Name = input.Name;
            }

            if (!string.IsNullOrWhiteSpace(input.PoolAddress))
            {
                flightSheet.PoolAddress = input.PoolAddress;
            }

            if (!string.IsNullOrWhiteSpace(input.ExtendedConfig))
            {
                flightSheet.ExtendedConfig = input.ExtendedConfig;
            }

            await db.SaveChangesAsync(ct);
            return Ok(flightSheet.Id);
        }
    }
}
