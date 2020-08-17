using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Company.Function
{
    public static class ManageKeyVaultSecrets
    {
        [FunctionName("ManageKeyVaultSecrets")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {

            var client = new SecretClient(new Uri("https://markappincc.vault.azure.net/"), new DefaultAzureCredential(), new SecretClientOptions());
            var key = await client.GetSecretAsync("marekdemo");
            var responseMessage = key.Value;
            return new OkObjectResult(responseMessage);
        }
    }
}
