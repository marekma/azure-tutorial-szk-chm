using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StockMarket.DataAccess;

namespace StockMarket.Function
{
    public class StockMarket
    {
        private readonly RateDataAccess dataAccess;
        public StockMarket(RateDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;

        }

        [FunctionName("StockMarket")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name) ?
                "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response." :
                $"Hello, {name}. This HTTP triggered function executed successfully.";

            await this.dataAccess.Create(new Entities.ExchangeRate() { value = 0.0025m, exchangeid = "pln", id = Guid.NewGuid().ToString(), to = "rub", date = DateTime.UtcNow });
            await this.dataAccess.Create(new Entities.ExchangeRate() { value = 1/0.0025m, exchangeid = "rub", id = Guid.NewGuid().ToString(), to = "pln", date = DateTime.UtcNow });

            return new OkObjectResult(responseMessage);
        }
    }
}