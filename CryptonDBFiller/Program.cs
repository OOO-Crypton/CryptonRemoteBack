using CryptonDBFiller;
using CryptonRemoteBack.Domain;
using CryptonRemoteBack.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql;

Console.WriteLine("Starting...");

NpgsqlConnectionStringBuilder builder = new("Server=localhost;Port=5432;Database=cryptondb;User Id=crypton_user;Password=crypton_pass;");
//NpgsqlConnectionStringBuilder builder = new("Server=localhost;Port=5432;Database=cryptondatabase;User Id=postgres;Password=postgres;");
DbContextOptionsBuilder<CryptonRemoteBackDbContext> optionsBuilder = new();

DbContextOptions<CryptonRemoteBackDbContext> options = optionsBuilder.UseNpgsql(builder.ConnectionString).Options;

CryptonRemoteBackDbContext db = new(options);

if (!db.Currencies.Any())
{
    Console.WriteLine("Add currencies...");
    foreach (string coin in Values.coins)
    {
        Currency currency = new()
        {
            Name = coin
        };
        db.Currencies.Add(currency);
    }
    db.SaveChanges();
}

if (!db.Miners.Any())
{
    Console.WriteLine("Add miners...");
    foreach ((string name, string readme, string container) in Values.miners)
    {
        Miner miner = new()
        {
            Name = name,
            ContainerName = container,
            MinerInfo = readme
        };
        db.Miners.Add(miner);
    }
    db.SaveChanges();
}
