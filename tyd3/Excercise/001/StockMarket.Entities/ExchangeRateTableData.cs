using Microsoft.Azure.Cosmos.Table;
using System;

namespace StockMarket.Entities
{
    public class ExchangeRateTableData : TableEntity
    {
        public ExchangeRateTableData()
        {

        }

        public ExchangeRateTableData(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {

        }
        public int Volume { get; set; }
        public int Buyers { get; set; }
    }
}
