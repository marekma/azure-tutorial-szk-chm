using Company.Function.Abstracts;
using Company.Function.Implementations;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Company.Function.Startup))]

namespace Company.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<IBlobProvider, AzureStorageBlobProvider>();
            builder.Services.AddSingleton<IQueueProvider, AzureStorageQueueProvider>();
        }
    }
}