using Npgsql;

namespace Discount.API.Extensions
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
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = _serviceProvider.GetService<ILogger>();

                try
                {
                    Logger.LogInformation("Migration postresql database.");
                    using var connection = new NpgsqlConnection
                        (configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
                    command.ExecuteNonQuery();

                    Logger.LogInformation("Migrated postresql database.");

                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "An error occurred while migrating the postresql database");

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
