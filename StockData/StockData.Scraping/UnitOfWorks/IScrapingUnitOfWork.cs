using StockData.Data;
using StockData.Scraping.Repositories;

namespace StockData.Scraping.UnitOfWorks
{
    public interface IScrapingUnitOfWork : IUnitOfWork
    {
        ICompanyRepository Companies { get; }
        IStockPriceRepository StockPrices { get; }
    }
}
