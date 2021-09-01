using StockData.Data;
using StockData.Scraping.Contexts;
using StockData.Scraping.Repositories;
using Microsoft.EntityFrameworkCore;

namespace StockData.Scraping.UnitOfWorks
{
    public class ScrapingUnitOfWork : UnitOfWork, IScrapingUnitOfWork
    {
        public ICompanyRepository Companies { get; private set; }
        public IStockPriceRepository StockPrices { get; private set; }

        public ScrapingUnitOfWork(IScrapingContext context, ICompanyRepository companies, IStockPriceRepository stockPrices) : base((DbContext)context)
        {
            Companies = companies;
            StockPrices = stockPrices;
        }
    }
}


