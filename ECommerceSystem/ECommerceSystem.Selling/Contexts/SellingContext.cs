using ECommerceSystem.Selling.Entites;
using Microsoft.EntityFrameworkCore;


namespace ECommerceSystem.Selling.Contexts
{
    public class SellingContext : DbContext, ISellingContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public SellingContext(string connectionString, string migrationAssemblyName)
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

        public DbSet<Product> Prodcuts { get; set; }
    }
}

