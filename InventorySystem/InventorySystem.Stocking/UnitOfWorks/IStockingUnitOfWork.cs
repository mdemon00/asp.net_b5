using InventorySystem.Data;
using InventorySystem.Stocking.Repositories;

namespace InventorySystem.Stocking.UnitOfWorks
{
    public interface IStockingUnitOfWork : IUnitOfWork
    {
        IProductRepository Products { get; }
        IStockRepository Stocks { get; }
    }
}
