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
using System.Text;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;

namespace Company.Function
{
    public static class DecryptionUsingKeyVault
    {
        [FunctionName("DecryptionUsingKeyVault")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var keyclient = new KeyClient(new Uri("https://markappincc.vault.azure.net/"), new DefaultAzureCredential());
            string payload = req.Query["payload"];

            byte[] toDecryptInBytes = Convert.FromBase64String(payload);
            KeyVaultKey key = await keyclient.GetKeyAsync("marekmaencrypt");
            CryptographyClient crypto = new CryptographyClient(key.Id, new DefaultAzureCredential());

            DecryptResult result = await crypto.DecryptAsync(EncryptionAlgorithm.RsaOaep256, toDecryptInBytes);

            return new OkObjectResult(Encoding.UTF8.GetString(result.Plaintext));
        }
    }
}
