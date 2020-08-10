using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace Company.Function
{
    class DataAccess<T>
    {
        protected readonly IConfiguration configuration;
        protected readonly ILogger logger;
        protected Database database;

        public DataAccess(IConfiguration configuration, ILogger logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task Configure()
        {
            this.logger.LogInformation("Cosmos db client initialization.");
            CosmosClient client = new CosmosClient(this.configuration.GetConnectionString("cosmosdb-stockmarket"));
            this.database = await client.CreateDatabaseIfNotExistsAsync("StockMarket");
            this.logger.LogInformation("Cosmos db client initialized.");
        }

        public async Task<T> Create(T item)
        {
            this.logger.LogInformation("Create an item in cosmos db.");

            dynamic testItem = new { id = Guid.NewGuid(), exchangeid = "usd", to = "pln", value = "4.56", date = DateTime.UtcNow };
            var response = await this.container.CreateItemAsync<T>(item);

            this.logger.LogInformation("Item created in cosmos db.");

            return response;
        }
    }
}
