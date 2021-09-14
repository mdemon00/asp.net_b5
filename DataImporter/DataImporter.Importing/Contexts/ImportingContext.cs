using DataImporter.Importing.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataImporter.Importing.Contexts
{
    public class ImportingContext : DbContext, IImportingContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ImportingContext(string connectionString, string migrationAssemblyName)
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
            }

            base.OnConfiguring(dbContextOptionsBuilder);
        }

        public DbSet<Group> Groups { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<Cell> Cells { get; set; }
    }
}
