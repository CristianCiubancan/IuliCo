using IuliCo.Core;
using IuliCo.Core.Enums;
using IuliCo.Database.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace IuliCo.Database
{
    public class AccountContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.{environment}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                var connectionString = configuration.GetConnectionString("AccountDatabase");
                if (string.IsNullOrEmpty(connectionString))
                {
                    AsyncLogger.Instance.LogAsync(LogLevel.Fatal, "No connection string configured.").ConfigureAwait(false);
                    throw new InvalidOperationException("No connection string configured.");
                }
                AsyncLogger.Instance.LogAsync(LogLevel.Info, "AccountContext created.").ConfigureAwait(false);
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

    }
}
