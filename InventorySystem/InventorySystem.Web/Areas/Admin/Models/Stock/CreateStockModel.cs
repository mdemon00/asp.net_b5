using Autofac;
using InventorySystem.Stocking.BuisnessObjects;
using InventorySystem.Stocking.Services;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Web.Areas.Admin.Models
{
    public class CreateStockModel
    {
        [Required, Range(1, 100000)]
        public int ProductId { get; set; }

        [Required, Range(1, 100000)]
        public double Quantity { get; set; }

        private readonly IStockService _stockService;
        public CreateStockModel()
        {
            _stockService = Startup.AutofacContainer.Resolve<IStockService>();
        }

        public CreateStockModel(IStockService stockService)
        {
            _stockService = stockService;
        }

        internal void CreateStock()
        {
            var stock = new Stock
            {
                ProductId = ProductId,
                Quantity = Quantity
            };

            _stockService.CreateStock(stock);
        }
    }
}
