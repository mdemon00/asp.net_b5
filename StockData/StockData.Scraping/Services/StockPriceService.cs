using StockData.Scraping.BuisnessObjects;
using StockData.Scraping.UnitOfWorks;
using StockData.Scraping.Exceptions;

namespace StockData.Scraping.Services
{
    public class StockPriceService : IStockPriceService
    {
        private readonly IScrapingUnitOfWork _scrapingUnitOfWork;
        public StockPriceService(IScrapingUnitOfWork scrapingUnitOfWork)
        {
            _scrapingUnitOfWork = scrapingUnitOfWork;
        }

        public void CreateStockPrice(StockPrice stockPrice)
        {
            if (stockPrice == null)
                throw new InvalidParameterException("StockPrice was not provided");

            _scrapingUnitOfWork.StockPrices.Add(new Entites.StockPrice
            {
                CompanyId = stockPrice.CompanyId,
                LastTradingPrice = stockPrice.LastTradingPrice,
                High = stockPrice.High,
                Low = stockPrice.Low,
                ClosePrice = stockPrice.ClosePrice,
                YesterdayClosePrice = stockPrice.YesterdayClosePrice,
                Change = stockPrice.Change,
                Trade = stockPrice.Trade,
                Value = stockPrice.Value,
                Volume = stockPrice.Volume
            });

            _scrapingUnitOfWork.Save();
        }

    }
}
