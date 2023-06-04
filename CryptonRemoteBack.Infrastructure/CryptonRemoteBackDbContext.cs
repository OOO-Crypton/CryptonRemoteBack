using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CryptonRemoteBack.Domain;

namespace CryptonRemoteBack.Infrastructure
{
    public sealed class CryptonRemoteBackDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
        public DbSet<Currency> Currencies { get; set; } = null!;
        public DbSet<Farm> Farms { get; set; } = null!;
        public DbSet<FlightSheet> FlightSheets { get; set; } = null!;
        public DbSet<Miner> Miners { get; set; } = null!;
        public DbSet<Pool> Pools { get; set; } = null!;
        public DbSet<PoolAddress> PoolAddresses { get; set; } = null!;
        public DbSet<Wallet> Wallets { get; set; } = null!;

        public CryptonRemoteBackDbContext(DbContextOptions<CryptonRemoteBackDbContext> options)
            : base(options) {   
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
        }

    }
}