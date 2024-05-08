using IuliCo.Core;
using IuliCo.Core.Enums;
using IuliCo.Database;
using IuliCo.Game.Sockets;
using Microsoft.EntityFrameworkCore;

namespace IuliCo.Game
{

    class Program
    {
        static async Task Main(string[] args)
        {
            // Create an instance of DbContext using the factory
            var dbContextFactory = new GameContextFactory();
            using (var dbContext = dbContextFactory.CreateDbContext(args))
            {
                // Apply pending migrations to the database
                await AsyncLogger.Instance.LogAsync(LogLevel.Info, "Applying pending migrations to the database.");
                await dbContext.Database.MigrateAsync();
                await AsyncLogger.Instance.LogAsync(LogLevel.Info, "Migrations applied.");
            }

            // Initialize and start the game server
            GameServer server = new GameServer(5186);
            await server.StartAsync();
        }
    }
}
