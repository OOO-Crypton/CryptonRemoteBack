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


        [HttpPost("/flight_sheets/add")]
        [Authorize]
        public async Task<ActionResult> AddFlightSheet(
            [FromForm] FlightSheetModel input,
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(input.Name)
                || input.MinerId == Guid.Empty
                || input.WalletId == Guid.Empty
                || input.PoolId == Guid.Empty)
            {
                return BadRequest("Fields Name, MinerId, WalletId or PoolId is empty");
            }

            Miner? miner = await db.Miners.FirstOrDefaultAsync(x => x.Id == input.MinerId, ct);
            Wallet? wallet = await db.Wallets.FirstOrDefaultAsync(x => x.Id == input.WalletId, ct);
            Pool? pool = await db.Pools.FirstOrDefaultAsync(x => x.Id == input.PoolId, ct);

            if (miner == null || wallet == null || pool == null)
            {
                return BadRequest($"Miner/wallet/pool not found");
            }

            FlightSheet flightSheet = new()
            {
                Name = input.Name,
                ExtendedConfig = input.ExtendedConfig,
                User = await db.Users.FirstAsync(x => x.Id == UserId.ToString(), ct),
                Miner = miner,
                Wallet = wallet,
                Pool = pool
            };

            await db.FlightSheets.AddAsync(flightSheet, ct);
            await db.SaveChangesAsync(ct);
            return Ok(flightSheet.Id);
        }


        [HttpPatch("/flight_sheets/delete")]
        [Authorize]
        public async Task<ActionResult> DeleteFlightSheet(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromBody] Guid flightSheetId,
            CancellationToken ct)
        {
            FlightSheet? flightSheet = await db.FlightSheets
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == flightSheetId, ct);

            if (flightSheet == null || flightSheet.User.Id != UserId)
            {
                return NotFound($"FlightSheet {flightSheetId} not found for current user");
            }

            db.FlightSheets.Remove(flightSheet);
            await db.SaveChangesAsync(ct);
            return Ok($"FlightSheet {flightSheet.Id} was deleted");
        }


        [HttpGet("/flight_sheets/all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FlightSheetView>>> GetAllFlightSheets(
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            List<FlightSheet> flightSheets = await db.FlightSheets
                .Include(x => x.User)
                .Include(x => x.Miner)
                .Include(x => x.Pool).ThenInclude(x => x.PoolAddresses)
                .Include(x => x.Wallet).ThenInclude(x => x.Currency)
                .Where(x => x.User.Id == UserId).ToListAsync(ct);

            return Ok(flightSheets.Select(x => new FlightSheetView(x)));
        }


        [HttpGet("/flight_sheets/{flightSheetId:Guid}")]
        [Authorize]
        public async Task<ActionResult<FlightSheetView>> GetFlightSheet(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] Guid flightSheetId,
            CancellationToken ct)
        {
            FlightSheet? flightSheet = await db.FlightSheets
                .Include(x => x.User)
                .Include(x => x.Miner)
                .Include(x => x.Pool).ThenInclude(x => x.PoolAddresses)
                .Include(x => x.Wallet).ThenInclude(x => x.Currency)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == flightSheetId, ct);

            if (flightSheet == null)
            {
                return NotFound($"FlightSheet {flightSheetId} not found for current user");
            }

            return Ok(new FlightSheetView(flightSheet));
        }


        [HttpGet("/flight_sheets/miners_list")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MinerView>>> GetMiners(
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            List<MinerView> miners = await db.Miners
                .Select(x => new MinerView(x)).ToListAsync(ct);
            return Ok(miners);
        }


        [HttpGet("/flight_sheets/pools_list")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PoolView>>> GetPools(
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            List<PoolView> pools = await db.Pools
                .Include(x => x.PoolAddresses)
                .Select(x => new PoolView(x)).ToListAsync(ct);
            return Ok(pools);
        }


        [HttpGet("/flight_sheets/miners_list/{minerId:Guid}")]
        [Authorize]
        public async Task<ActionResult<MinerView>> GetMinerInfo(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] Guid minerId,
            CancellationToken ct)
        {
            Miner? miner = await db.Miners.FirstOrDefaultAsync(x => x.Id == minerId, ct);
            if (miner == null)
            {
                return NotFound($"Miner {minerId} not found");
            }
            return Ok(new MinerView(miner));
        }


        [HttpPost("/flight_sheets/{flightSheetId:Guid}/edit")]
        [Authorize]
        public async Task<ActionResult> EditFlightSheet(
            [FromForm] FlightSheetModel input,
            [FromRoute] Guid flightSheetId,
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            FlightSheet? flightSheet = await db.FlightSheets
                .Include(x => x.User)
                .Include(x => x.Miner)
                .Include(x => x.Pool).ThenInclude(x => x.PoolAddresses)
                .Include(x => x.Wallet).ThenInclude(x => x.Currency)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == flightSheetId, ct);

            if (flightSheet == null)
            {
                return NotFound($"FlightSheet {flightSheetId} not found for current user");
            }

            Miner? miner = await db.Miners.FirstOrDefaultAsync(x => x.Id == input.MinerId, ct);
            Wallet? wallet = await db.Wallets.FirstOrDefaultAsync(x => x.Id == input.WalletId, ct);
            Pool? pool = await db.Pools.FirstOrDefaultAsync(x => x.Id == input.PoolId, ct);

            if (input.MinerId != Guid.Empty && miner != null)
            {
                flightSheet.Miner = miner;
            }

            if (input.WalletId != Guid.Empty && wallet != null)
            {
                flightSheet.Wallet = wallet;
            }

            if (input.PoolId != Guid.Empty && pool != null)
            {
                flightSheet.Pool = pool;
            }

            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                flightSheet.Name = input.Name;
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
