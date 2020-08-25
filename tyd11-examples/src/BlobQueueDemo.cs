using Company.Function.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Company.Function
{
    public class QueueStorageDemo
    {
        private readonly IQueueProvider queueProvider;

        public QueueStorageDemo(IQueueProvider queueProvider)
        {
            this.queueProvider = queueProvider;
        }

        [FunctionName("QueueStorageDemo")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            await this.queueProvider.SendMessage(req.Body);

            var response = await this.queueProvider.ReceiveMessage();

            return new OkObjectResult(response);
        }
    }
}
