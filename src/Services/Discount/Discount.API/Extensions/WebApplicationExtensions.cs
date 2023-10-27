using Npgsql;

namespace Discount.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void MigrationDatabase<TContext>(this WebApplication webApplication, int retry = 0)
        {
            using (var scope = webApplication.Services.CreateScope())
            {
                var services = webApplication.Services;
                var configuration = services.GetRequiredService<IConfiguration>();
                var loger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    loger.LogInformation("Migrating postgres database.");

                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon (Id SERIAL PRIMARY KEY,
                                                                 ProductName VARCHAR(24) NOT NULL,
                                                                 Description TEXT,
                                                                 Amount INT)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Iphone X', 'Iphone X discount', 150)";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Samsung S23 ultra', 'Samsung S23 ultra discount', 100)";
                    command.ExecuteNonQuery();

                    loger.LogInformation("Migrated postgres database.");
                }
                catch (Exception ex)
                {
                    loger.LogError(ex, "An error occurred white migrating the postgres database.");

                    if (retry < 50)
                    {
                        Thread.Sleep(2000);
                        MigrationDatabase<TContext>(webApplication, retry);
                    }
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
