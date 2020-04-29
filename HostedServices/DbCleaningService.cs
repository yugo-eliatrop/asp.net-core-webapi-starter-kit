using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using FindbookApi.Models;

namespace FindbookApi.HostedServices
{
    public class DbCleaningService : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly Context dbContext;
        private readonly ILogger<DbCleaningService> logger;

        public DbCleaningService(ILogger<DbCleaningService> logger)
        {
            this.logger = logger;
            dbContext = new Context();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("DB cleaning service is running");
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            return Task.CompletedTask;
        }

        private void DoWork(object State)
        {
            logger.LogInformation("Checking for outdated refresh tokens");
            RefreshToken[] tokens = dbContext.RefreshTokens.Where(t => t.ExpirationTime < DateTime.UtcNow).ToArray();
            logger.LogInformation($"{tokens.Length} outdated tokens found");
            if (tokens.Length == 0)
                return;
            dbContext.RefreshTokens.RemoveRange(tokens);
            dbContext.SaveChangesAsync();
            logger.LogInformation("Outdated tokens has been removed");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("DB cleaning service is stopping");
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}