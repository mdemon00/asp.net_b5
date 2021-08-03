using InventorySystem.Data;
using InventorySystem.Stocking.Contexts;
using InventorySystem.Stocking.Entites;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Stocking.Repositories
{
    public class StockRepository : Repository<Stock, int>, IStockRepository
    {
        public StockRepository(IStockingContext context) : base((DbContext)context)
        {

        }
    }
}
