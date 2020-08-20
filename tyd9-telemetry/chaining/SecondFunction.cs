using Company.Function.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Company.Function
{
    public class SecondFunction
    {        
        private readonly ITelemetry telemetry;
        private readonly IHeaderService headerService;

        public SecondFunction(ITelemetry telemetry, IHeaderService headerService)
        {
            this.telemetry = telemetry;
            this.headerService = headerService;
        }

        [FunctionName("SecondFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string operationId = this.headerService.GetCorrelationId(req);
            this.telemetry.SetUserContext("testId");
            this.telemetry.SetOperationContext(operationId);
            this.telemetry.Event(new Event
            {
                Name = "Inside SecondFunction - Receive and process data",
                Context = req.HttpContext,
                Additional = new Dictionary<string, string>
                {
                    ["ts"] = DateTime.Now.ToString()
                }
            });

            HttpClient client = new HttpClient();
            headerService.SetCorellationId(client.DefaultRequestHeaders, operationId);
            DateTime start = DateTime.Now;
            var response = await client.GetAsync("http://localhost:7071/api/ThirdFunction");
            var stop = (DateTime.Now - start);

            var dependency = await telemetry.Dependency(new Dependency()
            {
                Duration = stop,
                Name = "Calling ThirdFunction",
                Request = response.RequestMessage,
                Response = response,
                Data = await response.Content.ReadAsStringAsync()
            });

            var data = JsonConvert.DeserializeObject<Data>(dependency.Data);
            data.Content += " Added data from second function.";
            string output = JsonConvert.SerializeObject(data);

            return new OkObjectResult(output);
        }
    }
}
