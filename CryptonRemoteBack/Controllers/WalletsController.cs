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
    public class WalletsController : ControllerBase
    {
        private string UserId => User.GetUserId();

        public WalletsController() { }


        [HttpPost("/wallets/add")]
        [Authorize]
        public async Task<ActionResult> AddWallet(
            [FromForm] WalletModel input,
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(input.Name)
                || string.IsNullOrWhiteSpace(input.Address)
                || input.CurrencyId == Guid.Empty)
            {
                return BadRequest("Fields Name, Address or CurrencyId is empty");
            }

            Currency? currency = await db.Currencies.FirstOrDefaultAsync(x => x.Id == input.CurrencyId, ct);

            if (currency == null)
            {
                return BadRequest($"Currency {input.CurrencyId} not found");
            }

            Wallet wallet = new()
            {
                Name = input.Name,
                Address = input.Address,
                User = await db.Users.FirstAsync(x => x.Id == UserId.ToString(), ct),
                Currency = currency
            };

            await db.Wallets.AddAsync(wallet, ct);
            await db.SaveChangesAsync(ct);
            return Ok(wallet.Id);
        }


        [HttpPatch("/wallets/delete")]
        [Authorize]
        public async Task<ActionResult> DeleteWallet(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromBody] Guid walletId,
            CancellationToken ct)
        {
            Wallet? wallet = await db.Wallets
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == walletId, ct);

            if (wallet == null || wallet.User.Id != UserId)
            {
                return NotFound($"Wallet {walletId} not found for current user");
            }

            db.Wallets.Remove(wallet);
            await db.SaveChangesAsync(ct);
            return Ok($"Wallet {walletId} was deleted");
        }

        
        [HttpGet("/wallets/all")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WalletView>>> GetAllWallets(
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            List<Wallet> wallets = await db.Wallets
                .Include(x => x.User)
                .Include(x => x.Currency)
                .Where(x => x.User.Id == UserId).ToListAsync(ct);

            return Ok(wallets.Select(x => new WalletView(x)));
        }


        [HttpGet("/wallets/{walletId:Guid}")]
        [Authorize]
        public async Task<ActionResult<WalletView>> GetWallet(
            [FromServices] CryptonRemoteBackDbContext db,
            [FromRoute] Guid walletId,
            CancellationToken ct)
        {
            Wallet? wallet = await db.Wallets
                .Include(x => x.User)
                .Include(x => x.Currency)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == walletId, ct);

            if (wallet == null)
            {
                return NotFound($"Wallet {walletId} not found for current user");
            }

            return Ok(new WalletView(wallet));
        }


        [HttpGet("/wallets/currencies_list")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CurrencyView>>> GetCurrencies(
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            List<CurrencyView> currencies = await db.Currencies
                .Select(x => new CurrencyView(x)).ToListAsync(ct);
            return Ok(currencies);
        }


        [HttpPost("/wallets/{walletId:Guid}/edit")]
        [Authorize]
        public async Task<ActionResult> EditWallet(
            [FromForm] WalletModel input,
            [FromRoute] Guid walletId,
            [FromServices] CryptonRemoteBackDbContext db,
            CancellationToken ct)
        {
            Wallet? wallet = await db.Wallets
                .Include(x => x.User)
                .Include(x => x.Currency)
                .FirstOrDefaultAsync(x => x.User.Id == UserId && x.Id == walletId, ct);

            if (wallet == null)
            {
                return NotFound($"Wallet {walletId} not found for current user");
            }

            Currency? currency = await db.Currencies
                .FirstOrDefaultAsync(x => x.Id == input.CurrencyId, ct);
            if (input.CurrencyId != Guid.Empty && currency != null)
            {
                wallet.Currency = currency;
            }

            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                wallet.Name = input.Name;
            }

            if (!string.IsNullOrWhiteSpace(input.Address))
            {
                wallet.Address = input.Address;
            }

            await db.SaveChangesAsync(ct);
            return Ok(wallet.Id);
        }
    }
}
