using ECommerceSystem.Data;
using ECommerceSystem.Selling.Repositories;

namespace ECommerceSystem.Selling.UnitOfWorks
{
    public interface ISellingUnitOfWork : IUnitOfWork
    {
        IProductRepository Products { get; }
    }
}
