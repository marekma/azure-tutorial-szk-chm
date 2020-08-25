using Azure.Storage.Blobs;
using Company.Function.Abstracts;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Company.Function.Implementations
{
    public class AzureStorageBlobProvider : IBlobProvider
    {
        private readonly IConfiguration configuration;
        private readonly string connectionstring;
        private readonly Dictionary<string, BlobContainerClient> containers;

        public AzureStorageBlobProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionstring = this.configuration.GetConnectionString("BlobStorage");
            this.containers = new Dictionary<string, BlobContainerClient>();
        }

        public BlobContainerClient Initialize(string container)
        {
            if (!this.containers.ContainsKey(container))
            {
                this.containers[container] = new BlobContainerClient(this.connectionstring, container);
            }
            return this.containers[container];
        }

        public async Task Upload(Stream content, string blobname)
        {
            var blobClient = this.Initialize("test").GetBlobClient(blobname);
            
            await blobClient.UploadAsync(content);
        }

        public async Task<string> Download(string blobname)
        {
            var blobClient = this.Initialize("test").GetBlobClient(blobname);
            var response = await blobClient.DownloadAsync();
            var asString = string.Empty;
            using (StreamReader sr = new StreamReader(response.Value.Content))
            {
                asString = await sr.ReadToEndAsync();
            }
            return asString;
        }
    }
}
