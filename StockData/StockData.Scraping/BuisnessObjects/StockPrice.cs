
namespace StockData.Scraping.BuisnessObjects
{
    public class StockPrice
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
