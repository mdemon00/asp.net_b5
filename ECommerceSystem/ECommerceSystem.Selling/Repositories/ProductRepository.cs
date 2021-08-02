using ECommerceSystem.Data;
using ECommerceSystem.Selling.Contexts;
using ECommerceSystem.Selling.Entites;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSystem.Selling.Repositories
{
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {
        public ProductRepository(ISellingContext context) : base((DbContext)context)
        {

        }
    }
}
