using IuliCo.Core;
using IuliCo.Core.Enums;
using IuliCo.Database.Entities.Game;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace IuliCo.Database
{
    public class GameContext : DbContext
    {
        public DbSet<Player> Games { get; set; }  // Ensure the DbSet name represents the entity correctly, might be 'Players'?

        public GameContext(DbContextOptions<GameContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                var connectionString = configuration.GetConnectionString("GameDatabase");
                if (string.IsNullOrEmpty(connectionString))
                {
                    AsyncLogger.Instance.LogAsync(LogLevel.Fatal, "No connection string configured for GameDatabase.").ConfigureAwait(false);
                    throw new InvalidOperationException("No connection string configured for GameDatabase.");
                }

                AsyncLogger.Instance.LogAsync(LogLevel.Info, "GameContext created.").ConfigureAwait(false);
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
