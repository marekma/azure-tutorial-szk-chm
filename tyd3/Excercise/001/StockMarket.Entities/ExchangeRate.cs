using System;

namespace StockMarket.Entities
{
    public class ExchangeRate
    {
        public string id { get; set; }
        public string exchangeid { get; set; }
        public string to { get; set; }
        public DateTime date { get; set; }
        public decimal value { get; set; }
    }
}
