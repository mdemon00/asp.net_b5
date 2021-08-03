using InventorySystem.Stocking.BuisnessObjects;
using InventorySystem.Stocking.Exceptions;
using InventorySystem.Stocking.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventorySystem.Stocking.Services
{
    public class StockService : IStockService
    {
        private readonly IStockingUnitOfWork _stockingUnitOfWork;
        public StockService(IStockingUnitOfWork stockingUnitOfWork)
        {
            _stockingUnitOfWork = stockingUnitOfWork;
        }

        public void CreateStock(Stock stock)
        {
            if (stock == null)
                throw new InvalidParameterException("Stock was not provided");

            _stockingUnitOfWork.Stocks.Add(new Entites.Stock
            {
                ProductId = stock.ProductId,
                Quantity = stock.Quantity
            });

            _stockingUnitOfWork.Save();
        }

        public void DeleteStock(int id)
        {
            _stockingUnitOfWork.Stocks.Remove(id);
            _stockingUnitOfWork.Save();
        }

        public Stock GetStock(int id)
        {
            var stock = _stockingUnitOfWork.Stocks.GetById(id);

            if (stock == null) return null;

            return new Stock
            {
                Id = stock.Id,
                ProductId = stock.ProductId,
                Quantity = stock.Quantity
            };
        }

        public (IList<Stock> records, int total, int totalDisplay) GetStocks(int pageIndex, int pageSize, string searchText, string sortText)
        {
            int value = 0; 
            int.TryParse(searchText, out value); // determine whether a string represents a numeric value

            var stockData = _stockingUnitOfWork.Stocks.GetDynamic(value == 0
            ? null : x => x.ProductId.ToString().Contains(value.ToString()),
            sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from stock in stockData.data
                             select new Stock
                             {
                                 Id = stock.Id,
                                 ProductId = stock.ProductId,
                                 Quantity = stock.Quantity
                             }).ToList();

            return (resultData, stockData.total, stockData.totalDisplay);
        }

        public void UpdateStock(Stock stock)
        {
            if (stock == null)
                throw new InvalidOperationException("Stock is missing");

            var stockEntity = _stockingUnitOfWork.Stocks.GetById(stock.Id);

            if (stockEntity != null)
            {
                stockEntity.ProductId = stock.ProductId;
                stockEntity.Quantity = stock.Quantity;

                _stockingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find stock");
        }
    }
}
