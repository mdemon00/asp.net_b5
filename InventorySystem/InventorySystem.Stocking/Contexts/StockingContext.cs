using InventorySystem.Stocking.Entites;
using Microsoft.EntityFrameworkCore;

namespace InventorySystem.Stocking.Contexts
{
    public class StockingContext : DbContext, IStockingContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public StockingContext(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                dbContextOptionsBuilder.UseSqlServer(
                    _connectionString,
                    m => m.MigrationsAssembly(_migrationAssemblyName));
                base.OnConfiguring(dbContextOptionsBuilder);
            }

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Stock> Stocks { get; set; }
    }
}
