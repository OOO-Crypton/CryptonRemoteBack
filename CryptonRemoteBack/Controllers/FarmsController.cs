using CryptonRemoteBack.Domain;
using CryptonRemoteBack.Extensions;
using CryptonRemoteBack.Infrastructure;
using CryptonRemoteBack.Model;
using CryptonRemoteBack.Model.Models;
using CryptonRemoteBack.Model.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace CryptonRemoteBack.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FarmsController : ControllerBase
    {
        private string UserId => User.GetUserId();

        public FarmsController() { }


        [HttpPost("/farms/add")]
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


        [HttpDelete("/farms/delete/{farmId:int}")]
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


        [HttpGet("/farms/all")]
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


        [HttpGet("/farms/{farmId:int}")]
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


        [HttpGet("/farms/{farmId:int}/start")]
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
                return NotFound($"Farm {farmId} has no active flight sheet");
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


        [HttpGet("/farms/{farmId:int}/stop")]
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
                return NotFound($"Farm {farmId} has no active flight sheet");
            }

            if (await FarmsHelpers.StopFarm(farm.LocalSystemAddress,
                                            farm.ActiveFlightSheet.Miner.ContainerName))
            {
                return Ok("Success");
            }

            return BadRequest("Failed");
        }


        [HttpGet("/farms/{farmId:int}/restart")]
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
                return NotFound($"Farm {farmId} has no active flight sheet");
            }

            if (await FarmsHelpers.RestartFarm(farm.LocalSystemAddress,
                                               farm.ActiveFlightSheet.Miner.ContainerName))
            {
                return Ok("Success");
            }

            return BadRequest("Failed");
        }


        [HttpGet("/farms/{farmId:int}/get_monitorings")]
        [Authorize]
        public async Task<ActionResult<List<MinerMonitoringRecord>?>> GetFarmMonitorings(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] int farmId,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == farmId, ct);

            if (farm == null)
            {
                return NotFound($"Farm {farmId} not found for current user");
            }

            List<MinerMonitoringRecord>? result = await FarmsHelpers.GetMonitorings(farm.LocalSystemAddress);
            return result;
        }


        [HttpGet("/farms/get_all_farms_monitorings")]
        [Authorize]
        public async Task<ActionResult<List<FarmMonitoringView>>> GetAllFarmsMonitorings(
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            List<Farm> farms = await db.Farms
                .Include(x => x.User)
                .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Miner)
                .Include(x => x.ActiveFlightSheet).ThenInclude(x => x.Wallet)
                .Where(x => x.User.Id == UserId).AsNoTracking().ToListAsync(ct);

            if (farms == null || farms.Count == 0)
            {
                return NotFound($"Farms not found for current user");
            }

            List<FarmMonitoringView> result = new();
            foreach (Farm farm in farms)
            {
                result.Add(new(farm, await FarmsHelpers.GetMonitorings(farm.LocalSystemAddress)));
            }
            
            return result;
        }


        [HttpPatch("/farms/{farmId:int}/switch_flight_sheet")]
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


        [HttpPost("/farms/register")]
        public async Task<ActionResult> RegisterFarm(
            [FromForm] FarmRegisterModel input,
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            Farm? farm = await db.Farms
                .FirstOrDefaultAsync(x => x.LocalSystemID == input.LocalSystemID, ct);

            if (farm == null)
            {
                return NotFound($"Farm {input.LocalSystemID} not found");
            }

            farm.LocalSystemAddress = input.LocalSystemAddress;
            farm.SystemInfo = input.SystemInfo;
            await db.SaveChangesAsync(ct);
            return Ok(farm.Id);
        }


        [HttpPost("/farms/set_info")]
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


        [HttpGet("/farms/stats")]
        //[Authorize]
        public async Task GetStats([FromServices] CryptonRemoteBackDbContext db, CancellationToken ct)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                //List<Farm> farms = await db.Farms.Include(x => x.User)
                //    .Where(x => x.User.Id == UserId).AsNoTracking().ToListAsync(ct);
                List<Farm> farms = await db.Farms.Include(x => x.User)
                    .Where(x => x.User.Email == "a.rotov2001@mail.ru").AsNoTracking().ToListAsync(ct); //temp

                if (farms == null || farms.Count == 0)
                {
                    HttpContext.Response.StatusCode = 404;
                    return;
                }
                using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await FarmsHelpers.GetStats(webSocket, farms.Select(x => (x.Id, x.LocalSystemAddress)).ToList());
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }
    }
}