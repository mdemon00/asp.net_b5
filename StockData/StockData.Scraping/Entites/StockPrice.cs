using StockData.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockData.Scraping.Entites
{
    public class StockPrice : IEntity<int>
    {
        public int Id { get; set; }
        public string CompanyId { get; set; }
        public double LastTradingPrice { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double ClosePrice { get; set; }
        public double YesterdayClosePrice { get; set; }
        public double Change { get; set; }
        public double Trade { get; set; }
        public double Value { get; set; }
        public double Volume { get; set; }
    }
}
