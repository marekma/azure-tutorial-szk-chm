using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Company.Function
{
    internal class StockMarketDataAccess : DataAccess<Rate>
    {
        private Container container;

        public StockMarketDataAccess(IConfiguration configuration, ILogger logger):base(configuration, logger)
        {
        }

        public async Task Configure()
        {
            this.container = await base.database.CreateContainerIfNotExistsAsync(
                "ExchangeRate",
                "/exchangeid",
                400);
        }
    }
}
