using InventorySystem.Data;
using InventorySystem.Stocking.Entites;

namespace InventorySystem.Stocking.Repositories
{
    public interface IStockRepository : IRepository<Stock, int>
    {
    }    
}
