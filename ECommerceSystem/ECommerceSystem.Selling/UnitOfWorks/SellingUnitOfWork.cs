using ECommerceSystem.Data;
using ECommerceSystem.Selling.Contexts;
using ECommerceSystem.Selling.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSystem.Selling.UnitOfWorks
{
    public class SellingUnitOfWork : UnitOfWork, ISellingUnitOfWork
    {
        public IProductRepository Products { get; private set; }

        public SellingUnitOfWork(ISellingContext context, IProductRepository products) : base((DbContext)context)
        {
            Products = products;
        }
    }
}


