using Autofac;
using InventorySystem.Stocking.BuisnessObjects;
using InventorySystem.Stocking.Services;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Web.Areas.Admin.Models
{
    public class EditStockModel
    {
        [Required, Range(1, 50000000)]
        public int? Id { get; set; }

        [Required, Range(1, 100000)]
        public int? ProductId { get; set; }

        [Required, Range(1, 100000)]
        public double? Quantity { get; set; }

        private readonly IStockService _stockService;
        public EditStockModel()
        {
            _stockService = Startup.AutofacContainer.Resolve<IStockService>();
        }

        public EditStockModel(IStockService stockService)
        {
            _stockService = stockService;
        }

        public void LoadModelData(int id)
        {
            var stock = _stockService.GetStock(id);
            ProductId = stock?.ProductId;
            Quantity = stock?.Quantity;
        }

        internal void Update()
        {
            var stock = new Stock
            {
                Id = Id.HasValue ? Id.Value : 0,
                ProductId = ProductId.HasValue ? ProductId.Value : 0,
                Quantity = Quantity.HasValue ? Quantity.Value : 0,
            };
            _stockService.UpdateStock(stock);
        }
    }
}
