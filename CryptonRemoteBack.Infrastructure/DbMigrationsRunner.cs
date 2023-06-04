using CryptonRemoteBack.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Net.Sockets;

namespace CryptonRemoteBack.Infrastructure
{
    public static class DbMigrationsRunner
    {
        public static IHost RunDatabaseMigrations(this IHost host)
        {
            using IServiceScope serviceScope = host.Services.CreateScope();
            CryptonRemoteBackDbContext context = serviceScope.ServiceProvider.GetRequiredService<CryptonRemoteBackDbContext>();
            ILoggerFactory loggerFactory = serviceScope.ServiceProvider.GetRequiredService<ILoggerFactory>();

            ILogger logger = loggerFactory.CreateLogger("DatabaseMigrations");
            logger.LogInformation("Starting migrations...");

            try
            {
                Policy
                    .Handle<SocketException>()
                    .WaitAndRetry(10,
                        (tryNumber) => TimeSpan.FromSeconds(tryNumber),
                        (ex, delayTime) => logger.LogWarning(ex,
                            $"Connection to database server failed. Try after {delayTime.Seconds} seconds"))
                    .Execute(context.Database.Migrate);

                logger.LogInformation("Migrations ended. Awaiting cancel...");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Migrations failed");
                throw;
            }

            return host;
        }
    }
}
