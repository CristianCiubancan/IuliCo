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
            var basePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "../IuliCo.Database";
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("AccountDatabase");
            if (string.IsNullOrEmpty(connectionString))
            {
                AsyncLogger.Instance.LogAsync(Core.Enums.LogLevel.Fatal, "No connection string configured.").ConfigureAwait(false);
                throw new InvalidOperationException("No connection string configured.");
            }

            var builder = new DbContextOptionsBuilder<AccountContext>();
            builder.UseSqlServer(connectionString);
            AsyncLogger.Instance.LogAsync(Core.Enums.LogLevel.Info, "AccountContext created.").ConfigureAwait(false);
            return new AccountContext(builder.Options);
        }
    }

}