using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Company.Function.Startup))]

namespace Company.Function
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
            var cs = configuration.GetValue<string>("cosmosdb-stockmarket");

            return new CosmosClient(cs);
        }
    }
}