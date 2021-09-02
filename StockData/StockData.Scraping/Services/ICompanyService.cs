using StockData.Data;
using StockData.Scraping.BuisnessObjects;
using System.Collections.Generic;

namespace StockData.Scraping.Services
{
    public interface ICompanyService
    {
        void CreateCompany(Company company);
        bool ExistsCompany(string tradeCode);
    }
}
