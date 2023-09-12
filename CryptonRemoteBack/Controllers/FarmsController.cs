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
    public class FarmsController : ControllerBase
    {
        private string UserId => User.GetUserId();

        public FarmsController() { }


        [HttpPost("/farms/add")]
        [Authorize]
        public async Task<ActionResult> AddFarm(
            [FromForm] FarmFSModel input,
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            FlightSheet? fs = await db.FlightSheets
                .FirstOrDefaultAsync(x => x.Id == input.ActiveFlightSheetId, ct);

            Farm farm = new()
            {
                User = await db.Users.FirstAsync(x => x.Id == UserId.ToString(), ct),
                LocalSystemID = GetRandomString(7),
                ActiveFlightSheet = fs
            };

            await db.Farms.AddAsync(farm, ct);
            await db.SaveChangesAsync(ct);
            return Ok(farm.Id);
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
                .Include(x => x.ActiveFlightSheet)
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
                .Include(x => x.ActiveFlightSheet)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == farmId, ct);

            if (farm == null)
            {
                return NotFound($"Farm {farmId} not found for current user");
            }

            return Ok(new FarmView(farm));
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
    }
}
