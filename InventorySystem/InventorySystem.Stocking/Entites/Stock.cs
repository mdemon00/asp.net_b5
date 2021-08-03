
using InventorySystem.Data;

namespace InventorySystem.Stocking.Entites
{
    public class Stock : IEntity<int>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public double Quantity { get; set; }
    }
}
