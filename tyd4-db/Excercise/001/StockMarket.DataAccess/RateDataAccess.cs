using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StockMarket.Entities;
using System.Threading.Tasks;

namespace StockMarket.DataAccess
{
    public class RateDataAccess : DataAccess<ExchangeRate>
    {
        public RateDataAccess(IConfiguration configuration, ILoggerFactory loggerFactory) : base(configuration, loggerFactory)
        {
            base.Configure().GetAwaiter().GetResult();
        }
    }
}
