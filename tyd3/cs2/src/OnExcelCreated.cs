using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime When { get; set; }
    }
    public static class OnExcelCreated
    {
        [FunctionName("OnExcelCreated")]
        public static void Run([BlobTrigger("excels/{name}", Connection = "json2excel0formatter_STORAGE")] Stream input,
        [CosmosDB("Logs", "Excels", ConnectionStringSetting = "cosmosDB")] out dynamic output, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {input.Length} Bytes");

            output = new Document()
            {
                Id = Guid.NewGuid(),
                Name = name,
                When = DateTime.Now
            };
        }
    }
}
