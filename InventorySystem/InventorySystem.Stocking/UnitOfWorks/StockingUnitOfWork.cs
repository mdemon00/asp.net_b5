using InventorySystem.Data;
using InventorySystem.Stocking.Contexts;
using InventorySystem.Stocking.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Stocking.UnitOfWorks
{
    public class StockingUnitOfWork : UnitOfWork, IStockingUnitOfWork
    {
        public IProductRepository Products { get; private set; }

        public StockingUnitOfWork(IStockingContext context, IProductRepository products) : base((DbContext)context)
        {
            Products = products;
        }
    }
}
