using Company.Function.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Company.Function
{
    public class FirstFunction
    {
        private readonly ITelemetry telemetry;
        private readonly IHeaderService headerService;

        public FirstFunction(ITelemetry telemetry, IHeaderService headerService)
        {
            this.telemetry = telemetry;
            this.headerService = headerService;
        }

        [FunctionName("FirstFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var act = new Activity("Complex process");
            act.Start();
            telemetry.SetUserContext("testId");
            telemetry.SetOperationContext(act.Id);
            telemetry.Event(new Event
            {
                Name = "Get data: Inside FirstFunction",
                Context = req.HttpContext,
                Additional = new Dictionary<string, string>
                {
                    ["ts"] = DateTime.Now.ToString()
                }
            });
            telemetry.StartOperation(act.Id, "Get data");

            HttpClient client = new HttpClient();
            headerService.SetCorellationId(client.DefaultRequestHeaders, act.Id);
            DateTime start = DateTime.Now;
            var response = await client.GetAsync("http://localhost:7071/api/SecondFunction");
            var stop = (DateTime.Now - start);

            var dependency = await telemetry.Dependency(new Dependency()
            {
                Duration = stop,
                Name = "Calling SecondFunction",
                Request = response.RequestMessage,
                Response = response,
                Data = await response.Content.ReadAsStringAsync()
            });
            telemetry.StopOperation();
            var data = JsonConvert.DeserializeObject<Data>(dependency.Data);
            data.Content += " Added data from first function.";
            string output = JsonConvert.SerializeObject(data);
            return new OkObjectResult(output);
        }
    }
}
