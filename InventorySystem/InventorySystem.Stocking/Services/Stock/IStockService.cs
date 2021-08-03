using InventorySystem.Stocking.BuisnessObjects;
using System.Collections.Generic;

namespace InventorySystem.Stocking.Services
{
    public interface IStockService
    {
        void CreateStock(Stock stock);
        (IList<Stock> records, int total, int totalDisplay) GetStocks(int pageIndex, int pageSize,
    string searchText, string sortText);
        Stock GetStock(int id);
        void UpdateStock(Stock stock);
        void DeleteStock(int id);
    }
}
