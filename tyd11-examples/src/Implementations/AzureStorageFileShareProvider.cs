using Azure.Storage.Files.Shares;
using Company.Function.Abstracts;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Company.Function.Implementations
{
    public class AzureStorageFileShareProvider : IFileShareProvider
    {
        private readonly IConfiguration configuration;
        private readonly string connectionstring;
        private readonly Dictionary<string, ShareClient> shares;

        public AzureStorageFileShareProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionstring = this.configuration.GetConnectionString("AzureStorage");
            this.shares = new Dictionary<string, ShareClient>();
        }

        private ShareClient Initialize(string share)
        {
            if (!this.shares.ContainsKey(share))
            {
                this.shares[share] = new ShareClient(this.connectionstring, share);
            }
            return this.shares[share];
        }

        public async Task Upload(Stream content, string filename)
        {
            var shareClient = this.Initialize("test");
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(filename);
            await fileClient.CreateAsync(content.Length);
            await fileClient.UploadAsync(content);
        }

        public async Task<string> Download(string filename)
        {
            var shareClient = this.Initialize("test");
            var directoryClient = shareClient.GetRootDirectoryClient();
            var fileClient = directoryClient.GetFileClient(filename);
            var response = await fileClient.DownloadAsync();
            var asString = string.Empty;

            using (StreamReader sr = new StreamReader(response.Value.Content))
            {
                asString = await sr.ReadToEndAsync();
            }
            return asString;
        }
    }
}
