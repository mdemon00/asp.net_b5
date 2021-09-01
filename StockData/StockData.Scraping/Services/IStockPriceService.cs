using StockData.Scraping.BuisnessObjects;

namespace StockData.Scraping.Services
{
    public interface IStockPriceService
    {
        void CreateStockPrice(StockPrice stockPrice);
    }
}
