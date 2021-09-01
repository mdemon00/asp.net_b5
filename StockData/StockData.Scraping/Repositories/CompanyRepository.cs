using StockData.Data;
using StockData.Scraping.Contexts;
using StockData.Scraping.Entites;
using Microsoft.EntityFrameworkCore;

namespace StockData.Scraping.Repositories
{
    public class CompanyRepository : Repository<Company, int>, ICompanyRepository
    {
        public CompanyRepository(IScrapingContext context) : base((DbContext)context)
        {

        }
    }
}
