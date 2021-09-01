using StockData.Scraping.Entites;
using Microsoft.EntityFrameworkCore;


namespace StockData.Scraping.Contexts
{
    public class ScrapingContext : DbContext, IScrapingContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ScrapingContext(string connectionString, string migrationAssemblyName)
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

        public DbSet<Company> Companies { get; set; }
        public DbSet<StockPrice> StockPrices { get; set; }
    }
}

