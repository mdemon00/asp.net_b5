using StockData.Scraping.BuisnessObjects;
using StockData.Scraping.UnitOfWorks;
using StockData.Scraping.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockData.Scraping.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IScrapingUnitOfWork _scrapingUnitOfWork;
        public CompanyService(IScrapingUnitOfWork scrapingUnitOfWork)
        {
            _scrapingUnitOfWork = scrapingUnitOfWork;
        }

        public void CreateCompany(Company company)
        {
            if (company == null)
                throw new InvalidParameterException("Company was not provided");

            _scrapingUnitOfWork.Companies.Add(new Entites.Company
            {
                TradeCode = company.TradeCode
            });

            _scrapingUnitOfWork.Save();
        }

        public bool ExistsCompany(string tradeCode)
        {
            var exists = _scrapingUnitOfWork.Companies.GetAll().Any(x => x.TradeCode == tradeCode);

            return exists;
        }

    }
}
