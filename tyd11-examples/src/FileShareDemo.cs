using Company.Function.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Company.Function
{
    public class FileShareDemo
    {
        private readonly IFileShareProvider shareProvider;
        private readonly IFileNameGenerator fileNameGenerator;

        public FileShareDemo(IFileShareProvider shareProvider, IFileNameGenerator fileNameGenerator)
        {
            this.shareProvider = shareProvider;
            this.fileNameGenerator = fileNameGenerator;
        }

        [FunctionName("FileShareDemo")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var name = this.fileNameGenerator.ForJson();
            await this.shareProvider.Upload(req.Body, name);
            var response = await this.shareProvider.Download(name);
            return new OkObjectResult(response);
        }
    }
}
