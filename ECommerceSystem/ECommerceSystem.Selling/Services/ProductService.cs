using ECommerceSystem.Selling.BuisnessObjects;
using ECommerceSystem.Selling.Exceptions;
using ECommerceSystem.Selling.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceSystem.Selling.Services
{
    public class ProductService : IProductService
    {
        private readonly ISellingUnitOfWork _sellingUnitOfWork;
        public ProductService(ISellingUnitOfWork sellingUnitOfWork)
        {
            _sellingUnitOfWork = sellingUnitOfWork;
        }

        public void CreateProduct(Product product)
        {
            if (product == null)
                throw new InvalidParameterException("Product was not provided");

            _sellingUnitOfWork.Products.Add(new Entites.Product
            {
                Name = product.Name,
                Price = product.Price
            });

            _sellingUnitOfWork.Save();
        }

        public void DeleteProduct(int id)
        {
            _sellingUnitOfWork.Products.Remove(id);
            _sellingUnitOfWork.Save();
        }

        public Product GetProduct(int id)
        {
            var product = _sellingUnitOfWork.Products.GetById(id);

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
            var productData = _sellingUnitOfWork.Products.GetDynamic(string.IsNullOrWhiteSpace(searchText) ? null : x => x.Name.Contains(searchText),
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

            var productEntity = _sellingUnitOfWork.Products.GetById(product.Id);

            if (productEntity != null)
            {
                productEntity.Name = product.Name;
                productEntity.Price = product.Price;

                _sellingUnitOfWork.Save();
            }
            else
                throw new InvalidOperationException("Couldn't find product");
        }
    }
}
