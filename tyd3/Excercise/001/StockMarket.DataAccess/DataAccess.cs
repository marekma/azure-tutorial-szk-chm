using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using StockMarket.Entities;

namespace StockMarket.DataAccess
{
    public abstract class DataAccess<T>
    {
        protected readonly IConfiguration configuration;
        protected readonly ILogger logger;
        protected Container container;

        public DataAccess(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            this.configuration = configuration;
            this.logger = loggerFactory.CreateLogger<DataAccess<T>>();
        }

        public virtual async Task Configure()
        {
            this.logger.LogInformation("Cosmos client, database and container initialization.");

            CosmosClient client = new CosmosClient(this.configuration.GetConnectionString($"cosmosdb-stockmarket"));
            Database database = await client.CreateDatabaseIfNotExistsAsync("StockMarket");
            this.container = await database.CreateContainerIfNotExistsAsync(
                nameof(ExchangeRate),
                "/exchangeid",
                400);
            this.logger.LogInformation("Cosmos client, database and container initialized.");
        }

        public async Task<T> Create(T item)
        {
            this.logger.LogInformation("Create an item in cosmos db.");

            var response = await this.container.CreateItemAsync<T>(item);

            this.logger.LogInformation("Item created in cosmos db.");

            return (T)response;
        }
    }
}
