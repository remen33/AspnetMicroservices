using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Ordering.Infrastructure.Persistence;

namespace Ordering.API.Extensions
{
    public class HostExtensions : IHostedService 
    {
        private readonly IServiceProvider _serviceProvider;
        public HostExtensions(IServiceProvider serviceProvider, ILogger<HostExtensions> logger)
        {
            _serviceProvider = serviceProvider;
            Logger = logger;
        }

        public ILogger<HostExtensions> Logger { get; }

        public Task StartAsync(CancellationToken cancellationToken) 
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<OrderContext>>();
                var context = services.GetService<OrderContext>();
                
                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(OrderContext).Name);

                    context.Database.Migrate();

                    var loggerSeeder = services.GetService<ILogger<OrderContextSeed>>();
                    OrderContextSeed.SeedAsync(context, loggerSeeder).Wait();

                    logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(OrderContext).Name);
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(OrderContext).Name);

                }
            }
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
