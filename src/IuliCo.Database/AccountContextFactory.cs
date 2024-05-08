using IuliCo.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IuliCo.Database
{
    public class AccountContextFactory : IDesignTimeDbContextFactory<AccountContext>
    {
        public AccountContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("AccountDatabase");
            if (string.IsNullOrEmpty(connectionString))
            {
                AsyncLogger.Instance.LogAsync(Core.Enums.LogLevel.Fatal, "No connection string configured.").ConfigureAwait(false);
                throw new InvalidOperationException("No connection string configured.");
            }

            var builder = new DbContextOptionsBuilder<AccountContext>();
            builder.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 21))
            );
            AsyncLogger.Instance.LogAsync(Core.Enums.LogLevel.Info, "AccountContext created.").ConfigureAwait(false);
            return new AccountContext(builder.Options);
        }
    }

}
