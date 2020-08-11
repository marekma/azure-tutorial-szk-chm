using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using StockMarket.Entities;

namespace Company.Function
{
    public class StockMarketPrices
    {
        private readonly IConfiguration configuration;

        public StockMarketPrices(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [FunctionName("StockMarketPrices")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var storageAccount = CloudStorageAccount.Parse(this.configuration.GetConnectionString("stockmarket-table"));
            var client = storageAccount.CreateCloudTableClient();
            var table = client.GetTableReference("StockMarketPrices");

            await table.CreateIfNotExistsAsync();
            var rate = new ExchangeRateTableData("pln", Guid.NewGuid().ToString())
            {
                Volume = 10000,
                Buyers = 1
            };

            var operation = TableOperation.InsertOrMerge(rate);
            await table.ExecuteAsync(operation);

            operation = TableOperation.Retrieve<ExchangeRateTableData>(rate.PartitionKey, null);
            rate = (await table.ExecuteAsync(operation)).Result as ExchangeRateTableData;

            return new OkObjectResult(rate);
        }
    }
}
