using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using StockMarket.Entities;
using System;
using System.Threading.Tasks;

namespace Company.Function
{
    public class StockMarketRedis
    {
        private readonly IConfiguration configuration;

        public StockMarketRedis(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        [FunctionName("StockMarketRedis")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var key = "StockMarket-9bb88418-5e50-4f4a-aa97-3b1624874106";
            var redisConnect = await ConnectionMultiplexer.ConnectAsync(this.configuration.GetConnectionString("stockmarket-redis"));
            var redisCache = redisConnect.GetDatabase();
            var redisValue = redisCache.StringGet(key);
            ExchangeRateTableData rate;

            if (!redisValue.HasValue)
            {
                var storageAccount = CloudStorageAccount.Parse(this.configuration.GetConnectionString("stockmarket-table"));
                var client = storageAccount.CreateCloudTableClient();
                var table = client.GetTableReference("StockMarketPrices");
                await table.CreateIfNotExistsAsync();
                var operation = TableOperation.Retrieve<ExchangeRateTableData>("pln", "9bb88418-5e50-4f4a-aa97-3b1624874106");
                rate = (await table.ExecuteAsync(operation)).Result as ExchangeRateTableData;
                await redisCache.StringSetAsync(key, JsonConvert.SerializeObject(rate), TimeSpan.FromMinutes(5));
            }
            else
            {
                rate = JsonConvert.DeserializeObject<ExchangeRateTableData>(redisValue.ToString());
            }

            return new OkObjectResult(rate);
        }
    }
}
