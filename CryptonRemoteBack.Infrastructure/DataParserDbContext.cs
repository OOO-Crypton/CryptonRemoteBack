using CryptonRemoteBack.Domain.CoinsDatabase;
using Microsoft.EntityFrameworkCore;

namespace CryptonRemoteBack.Infrastructure
{
    public sealed class DataParserDbContext : DbContext
    {
        public DbSet<Algorithm> Algorithms { get; set; }
        public DbSet<Coin> Coins { get; set; }
        public DbSet<Monitoring> Monitorings { get; set; }

        public DataParserDbContext(DbContextOptions<DataParserDbContext> options)
            : base(options)
        {
        }

    }
}
