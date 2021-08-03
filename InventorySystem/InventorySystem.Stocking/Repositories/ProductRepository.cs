using InventorySystem.Data;
using InventorySystem.Stocking.Contexts;
using InventorySystem.Stocking.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystem.Stocking.Repositories
{
    public class ProductRepository : Repository<Product, int>, IProductRepository
    {
        public ProductRepository(IStockingContext context) : base((DbContext)context)
        {

        }
    }
}
