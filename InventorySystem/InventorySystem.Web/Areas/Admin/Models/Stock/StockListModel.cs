using Autofac;
using InventorySystem.Stocking.Services;
using InventorySystem.Web.Models;
using System.Linq;

namespace InventorySystem.Web.Areas.Admin.Models
{
    public class StockListModel
    {
        private readonly IStockService _stockService;
        public StockListModel()
        {
            _stockService = Startup.AutofacContainer.Resolve<IStockService>();
        }

        public StockListModel(IStockService stockService)
        {
            _stockService = stockService;
        }

        internal object GetStocks(DataTablesAjaxRequestModel tableModel)
        {
            var data = _stockService.GetStocks(tableModel.PageIndex,
                tableModel.PageSize,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "ProductId", "Quantity" }));

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                                record.ProductId.ToString(),
                                record.Quantity.ToString(),
                                record.Id.ToString()
                        }
                    ).ToArray()
            };
        }

        internal void Delete(int id)
        {
            _stockService.DeleteStock(id);
        }
    }
}
