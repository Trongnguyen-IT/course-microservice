using Dapper;
using Discount.API.AppSettings;
using Discount.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);
        Task<bool> CreateDiscount(Coupon coupon);
        Task<bool> UpdateDiscount(Coupon coupon);
        Task<bool> DeleteDiscount(string productName);
    }

    public class DiscountRepository : IDiscountRepository
    {
        private readonly DatabaseSettings _settings;

        public DiscountRepository(IOptionsMonitor<DatabaseSettings> settings)
        {
            _settings = settings.CurrentValue;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_settings.ConnectionString);

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon == null)
            {
                return new Coupon
                {
                    ProductName = "No discount",
                    Amount = 0,
                    Description = "No discount Desc"
                };
            }
            return coupon;
        }

        [HttpPost]
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_settings.ConnectionString);

            var affected = await connection.ExecuteAsync
                ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { coupon.ProductName, coupon.Description, coupon.Amount });

            return affected == 1;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_settings.ConnectionString);

            var affected = await connection.ExecuteAsync
                ("DELETE FROM Coupon WHERE ProductName = @ProductName",
                new { ProductName = productName });

            return affected == 1;
        }

        [HttpPut]
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_settings.ConnectionString);

            var affected = await connection.ExecuteAsync
                ("UPDATE Coupon SET ProductName = @ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

            return affected == 1;
        }
    }
}
