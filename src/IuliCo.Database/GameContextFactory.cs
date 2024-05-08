using IuliCo.Core;
using IuliCo.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IuliCo.Database
{
    public class GameContextFactory : IDesignTimeDbContextFactory<GameContext>
    {
        public GameContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{environment}.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("GameDatabase");
            if (string.IsNullOrEmpty(connectionString))
            {
                AsyncLogger.Instance.LogAsync(LogLevel.Fatal, "No connection string configured for GameDatabase.").ConfigureAwait(false);
                throw new InvalidOperationException("No connection string configured for GameDatabase.");
            }

            var builder = new DbContextOptionsBuilder<GameContext>();
            builder.UseMySql(
                    connectionString,
                    new MySqlServerVersion(new Version(8, 0, 21))
                );
            AsyncLogger.Instance.LogAsync(LogLevel.Info, "GameContext created.").ConfigureAwait(false);
            return new GameContext(builder.Options);
        }
    }
}
