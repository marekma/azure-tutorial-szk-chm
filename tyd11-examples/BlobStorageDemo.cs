using Company.Function.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Company.Function
{
    public class BlobStorageDemo
    {
        private readonly IBlobProvider blobProvider;

        public BlobStorageDemo(IBlobProvider blobProvider)
        {
            this.blobProvider = blobProvider;
        }

        [FunctionName("BlobStorageDemo")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var response = string.Empty;

            var blobname = Guid.NewGuid().ToString();
            await this.blobProvider.Upload(req.Body, blobname);
            response = await this.blobProvider.Download(blobname);

            return new OkObjectResult(response);
        }
    }
}
