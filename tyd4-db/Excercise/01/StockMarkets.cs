using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Company.Function
{
    public static class StockMarkets
    {
        [FunctionName("StockMarkets")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            //CosmosClient client = new CosmosClient("AccountEndpoint=https://tutorialcosmos.documents.azure.com:443/;AccountKey=gmUG9j2S7J9KSK5kl4kq2hB6PYP8PCWf5U82mL88PzcLXllWTQl1jgcFb8vsdkHiTAWiCT0PAf15wnOJNN0alw==;");
            //Database database = await client.CreateDatabaseIfNotExistsAsync("StockMarket");
            //Container container = await database.CreateContainerIfNotExistsAsync(
            //    "ExchangeRate",
            //    "/exchangeid",
            //    400);

            // Create an item
            dynamic testItem = new { id = Guid.NewGuid(), exchangeid = "usd", to = "pln", value = "4.56", date = DateTime.UtcNow };
            var createResponse = await container.CreateItemAsync(testItem);
            testItem = new { id = Guid.NewGuid(), exchangeid = "usd", to = "euro", value = "0.98", date = DateTime.UtcNow };
            createResponse = await container.CreateItemAsync(testItem);

            List<Rate> list = new List<Rate>();

            // Query for an item
            using (FeedIterator<Rate> feedIterator = container.GetItemQueryIterator<Rate>("select * from T where T.exchangeid = 'usd'"))
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<Rate> response = await feedIterator.ReadNextAsync();
                    foreach (var item in response)
                    {
                        list.Add(item);
                        Console.WriteLine(item);
                    }
                }
            }
            return new OkObjectResult(list);
        }
    }
}
