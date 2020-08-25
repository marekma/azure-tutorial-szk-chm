using Azure.Storage.Queues;
using Company.Function.Abstracts;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Company.Function.Implementations
{
    public class AzureStorageQueueProvider : IQueueProvider
    {
        private readonly IConfiguration configuration;
        private readonly string connectionstring;
        private readonly Dictionary<string, QueueClient> queues;

        public AzureStorageQueueProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionstring = this.configuration.GetConnectionString("AzureStorage");
            this.queues = new Dictionary<string, QueueClient>();
        }

        private QueueClient Initialize(string queue)
        {
            if (!this.queues.ContainsKey(queue))
            {
                this.queues[queue] = new QueueClient(this.connectionstring, queue);
            }
            return this.queues[queue];
        }

        public async Task<string> ReceiveMessage()
        {
            var client = this.Initialize("test");
            var messages = await client.ReceiveMessagesAsync();
            return messages.Value[0].MessageText;
        }

        public async Task SendMessage(Stream message)
        {
            var client = this.Initialize("test");
            using (StreamReader sr = new StreamReader(message))
            {
                await client.SendMessageAsync(await sr.ReadToEndAsync());
            }
        }
    }
}
