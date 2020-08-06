using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;

namespace Cybercom.Functions
{
    public class GetInfo
    {
        private readonly CosmosClient client;

        public GetInfo(CosmosClient client)
        {
            this.client = client;
        }

        [FunctionName("GetInfo")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var excels = this.client
                .GetDatabase("Logs")
                .GetContainer("Excels")
                .GetItemQueryIterator<Document>("select * from c");

            List<Document> docs = new List<Document>();

            while (excels.HasMoreResults)
            {
                FeedResponse<Document> currentResultSet = await excels.ReadNextAsync();
                foreach (Document family in currentResultSet)
                {
                    docs.Add(family);
                }
            }

            return new OkObjectResult(docs);
        }
    }

    class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime When { get; set; }
    }
}
