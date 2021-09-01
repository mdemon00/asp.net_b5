using StockData.Data;
using StockData.Scraping.Contexts;
using StockData.Scraping.Entites;
using Microsoft.EntityFrameworkCore;

namespace StockData.Scraping.Repositories
{
    public class StockPriceRepository : Repository<StockPrice, int>, IStockPriceRepository
    {
        public StockPriceRepository(IScrapingContext context) : base((DbContext)context)
        {

        }
    }
}
