using StockData.Data;
using StockData.Scraping.Entites;

namespace StockData.Scraping.Repositories
{
    public interface IStockPriceRepository : IRepository<StockPrice, int>
    {
        StockPrice GetStockPrice(string companyId);
    }
}
