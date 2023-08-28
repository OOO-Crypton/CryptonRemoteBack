using CryptonDBFiller;
using CryptonRemoteBack.Domain;
using CryptonRemoteBack.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql;

Console.WriteLine("Starting...");

NpgsqlConnectionStringBuilder builder = new("Server=localhost;Port=5432;Database=cryptondb;User Id=cryptonuser;Password=cryptonpass;");
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
    foreach ((string name, string readme) in Values.miners)
    {
        Miner miner = new()
        {
            Name = name,
            MinerInfo = readme
        };
        db.Miners.Add(miner);
    }
    db.SaveChanges();
}

if (!db.Pools.Any())
{
    Console.WriteLine("Add pools...");
    Pool pool1 = new()
    {
        Name = "TestPool1"
    };
    PoolAddress pool1address1 = new()
    {
        Address = "pool1.miner1.test:1234"
    };
    PoolAddress pool1address2 = new()
    {
        Address = "pool1.miner2.test:4321"
    };
    pool1.PoolAddresses.Add(pool1address1);
    pool1.PoolAddresses.Add(pool1address2);
    db.Pools.Add(pool1);

    Pool pool2 = new()
    {
        Name = "TestPool2"
    };
    PoolAddress pool2address1 = new()
    {
        Address = "pool2.miner1.test:1234"
    };
    PoolAddress pool2address2 = new()
    {
        Address = "pool2.miner2.test:4321"
    };
    pool2.PoolAddresses.Add(pool2address1);
    pool2.PoolAddresses.Add(pool2address2);
    db.Pools.Add(pool2);

    db.SaveChanges();
}
