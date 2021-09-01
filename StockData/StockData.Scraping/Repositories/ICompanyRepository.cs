using StockData.Data;
using StockData.Scraping.Entites;

namespace StockData.Scraping.Repositories
{
    public interface ICompanyRepository : IRepository<Company, int>
    {
    }
}
