using StockData.Scraping.BuisnessObjects;
using System.Collections.Generic;

namespace StockData.Scraping.Services
{
    public interface IStockPriceService
    {
        void CreateStockPrice(StockPrice stockPrice);
    }
}
