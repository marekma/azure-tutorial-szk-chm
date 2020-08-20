using Company.Function.DataModels;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Company.Function
{
    public class ThirdFunction
    {
        private readonly ITelemetry telemetry;
        private readonly IHeaderService headerService;

        public ThirdFunction(ITelemetry telemetry, IHeaderService headerService)
        {
            this.telemetry = telemetry;
            this.headerService = headerService;
        }

        [FunctionName("ThirdFunction")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string operationId = this.headerService.GetCorrelationId(req);
            this.telemetry.SetUserContext("testId");
            this.telemetry.SetOperationContext(operationId);
            this.telemetry.Event(new Event
            {
                Name = "Inside ThirdFunction - Return data",
                Context = req.HttpContext,
                Additional = new Dictionary<string, string>
                {
                    ["ts"] = DateTime.Now.ToString()
                }
            });

            string output = JsonConvert.SerializeObject(new Data() { Content = "Content data from third function.", Id = 1 });
            return new OkObjectResult(output);
        }
    }
}
