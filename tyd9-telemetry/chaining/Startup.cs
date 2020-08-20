using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
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
            builder.Services.AddSingleton<ITelemetry>(services =>
            {
                IConfiguration config = services.GetService<IConfiguration>();
                return ApplicationInsightTelemetry.Create(new TelemetryClient(new TelemetryConfiguration()
                {
                    InstrumentationKey = config.GetValue<string>("ai_instrumentationKey"),
                }));
            });

            builder.Services.AddSingleton<IHeaderService>(new HeaderService());
            builder.Services.AddSingleton<IBodyService>(new BodyService());
        }
    }
}