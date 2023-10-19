using Catalog.API.AppSettings;
using Catalog.API.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
    }
    public class CatalogContext : ICatalogContext
    {
        private readonly DatabaseSettings _databaseSettings;

        public CatalogContext(IOptionsMonitor<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.CurrentValue;
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            Products = database.GetCollection<Product>(_databaseSettings.CollectionName);

            CatalogContextSeed.SeedData(Products);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
