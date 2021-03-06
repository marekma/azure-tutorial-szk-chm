using Company.Function.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Company.Function
{
    public class BlobStorageDemo
    {
        private readonly IBlobProvider blobProvider;
        private readonly IFileNameGenerator fileNameGenerator;

        public BlobStorageDemo(IBlobProvider blobProvider, IFileNameGenerator fileNameGenerator)
        {
            this.blobProvider = blobProvider;
            this.fileNameGenerator = fileNameGenerator;
        }

        [FunctionName("BlobStorageDemo")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var name = this.fileNameGenerator.ForJson();
            await this.blobProvider.Upload(req.Body, name);
            var response = await this.blobProvider.Download(name);

            return new OkObjectResult(response);
        }
    }
}
