using Autofac;
using InventorySystem.Stocking.BuisnessObjects;
using InventorySystem.Stocking.Services;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Web.Areas.Admin.Models
{
    public class CreateProductModel
    {
        [Required, MaxLength(50, ErrorMessage = "Name should be less than 50 charcaters")]
        public string Name { get; set; }
        [Required, Range(1, 100000000)]
        public double Price { get; set; }

        private readonly IProductService _productService;
        public CreateProductModel()
        {
            _productService = Startup.AutofacContainer.Resolve<IProductService>();
        }

        public CreateProductModel(IProductService productService)
        {
            _productService = productService;
        }

        internal void CreateProduct()
        {
            var product = new Product
            {
                Name = Name,
                Price = Price
            };

            _productService.CreateProduct(product);
        }
    }
}
