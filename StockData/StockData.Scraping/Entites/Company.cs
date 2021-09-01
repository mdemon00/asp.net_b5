using StockData.Data;

namespace StockData.Scraping.Entites
{
    public class Company : IEntity<int>
    {
        public int Id { get; set; }
        public string TradeCode { get; set; }
    }
}
