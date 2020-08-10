using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockMarket.DataAccess;

[assembly: FunctionsStartup(typeof(StockMarket.Function.Startup))]

namespace StockMarket.Function
{
    public class Startup : FunctionsStartup
    {
        public Startup()
        {
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            //builder.Services.AddTransient<RateDataAccess>((x) => Register(x));
            builder.Services.AddTransient<RateDataAccess>();
            builder.Services.AddLogging();
        }

        private RateDataAccess Register(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
            var rda = new RateDataAccess(configuration, loggerFactory);
            rda.Configure().GetAwaiter().GetResult();
            return rda;
        }
    }
}