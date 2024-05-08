
using IuliCo.Account.Sockets;
using IuliCo.Core;
using IuliCo.Core.Enums;
using IuliCo.Database;
using Microsoft.EntityFrameworkCore;

namespace IuliCo.Account
{

    class Program
    {
        static async Task Main(string[] args)
        {
            // Create an instance of DbContext using the factory
            var dbContextFactory = new AccountContextFactory();
            using (var dbContext = dbContextFactory.CreateDbContext(args))
            {
                await AsyncLogger.Instance.LogAsync(LogLevel.Info, "Applying pending migrations to the database.");
                // Apply pending migrations to the database
                await dbContext.Database.MigrateAsync();
                await AsyncLogger.Instance.LogAsync(LogLevel.Info, "Migrations applied.");
            }


            AccountServer server = new AccountServer(9959);
            await server.StartAsync();
        }
    }
}