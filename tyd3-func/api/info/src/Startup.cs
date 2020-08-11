using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Cybercom.Functions.Startup))]

namespace Cybercom.Functions
{
    public class Startup : FunctionsStartup
    {
        public Startup()
        {
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddTransient<CosmosClient>((x) => Register(x));
        }

        private CosmosClient Register(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var cs = configuration.GetValue<string>("cosmosDB");

            return new CosmosClient(cs);
        }
    }
}