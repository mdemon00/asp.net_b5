using InventorySystem.Stocking.BuisnessObjects;
using InventorySystem.Stocking.Exceptions;
using InventorySystem.Stocking.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InventorySystem.Stocking.Services
{
    public class ProductService : IProductService
    {
        private readonly IStockingUnitOfWork _StockingUnitOfWork;
        public ProductService(IStockingUnitOfWork StockingUnitOfWork)
        {
            _StockingUnitOfWork = StockingUnitOfWork;
        }

        public void CreateProduct(Product product)
        {
            if (product == null)
                throw new InvalidParameterException("Product was not provided");

            _StockingUnitOfWork.Products.Add(new Entites.Product
            {
                Name = product.Name,
                Price = product.Price
            });

            _StockingUnitOfWork.Save();
        }

        public void DeleteProduct(int id)
        {
            _StockingUnitOfWork.Products.Remove(id);
            _StockingUnitOfWork.Save();
        }

        public Product GetProduct(int id)
        {
            var product = _StockingUnitOfWork.Products.GetById(id);

            if (product == null) return null;

            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };
        }

        public (IList<Product> records, int total, int totalDisplay) GetProducts(int pageIndex, int pageSize, string searchText, string sortText)
        {
            var productData = _StockingUnitOfWork.Products.GetDynamic(string.IsNullOrWhiteSpace(searchText) ? null : x => x.Name.Contains(searchText),
                sortText, string.Empty, pageIndex, pageSize);

            var resultData = (from product in productData.data
                              select new Product
                              {
                                  Id = product.Id,
                                  Name = product.Name,
                                  Price = product.Price
                              }).ToList();
            return (resultData, productData.total, productData.totalDisplay);
        }

        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new InvalidOperationException("Product is missing");

            var productEntity = _StockingUnitOfWork.Products.GetById(product.Id);

            if (productEntity != null)
            {
                productEntity.Name = product.Name;
                productEntity.Price = product.Price;

                _StockingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find product");
        }
    }
}
