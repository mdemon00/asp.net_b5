using ECommerceSystem.Data;

namespace ECommerceSystem.Selling.Entites
{
    public class Product : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }
}
