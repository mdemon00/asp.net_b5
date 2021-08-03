using Autofac;
using InventorySystem.Stocking.BuisnessObjects;
using InventorySystem.Stocking.Services;
using System.ComponentModel.DataAnnotations;

namespace InventorySystem.Web.Areas.Admin.Models
{
    public class EditProductModel
    {
        [Required, Range(1, 50000000)]
        public int? Id { get; set; }
        [Required, MaxLength(50, ErrorMessage = "Name should be less than 50 charcaters")]
        public string Name { get; set; }
        [Required, Range(1, 100000000)]
        public double? Price { get; set; }

        private readonly IProductService _productService;
        public EditProductModel()
        {
            _productService = Startup.AutofacContainer.Resolve<IProductService>();
        }

        public EditProductModel(IProductService productService)
        {
            _productService = productService;
        }


        public void LoadModelData(int id)
        {
            var product = _productService.GetProduct(id);
            Id = product?.Id;
            Name = product?.Name;
            Price = product?.Price;
        }

        internal void Update()
        {
            var product = new Product
            {
                Id = Id.HasValue ? Id.Value : 0,
                Name = Name,
                Price = Price.HasValue ? Price.Value : 0,
            };
            _productService.UpdateProduct(product);
        }
    }
}
