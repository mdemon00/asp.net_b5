using Microsoft.EntityFrameworkCore;
using SocialNetwork.Registering.Entites;

namespace SocialNetwork.Registering.Contexts
{
    public class RegisteringContext : DbContext, IRegisteringContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public RegisteringContext(string connectionString, string migrationAssemblyName)
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

        public DbSet<Member> Member { get; set; }
        public DbSet<Photo> Photo { get; set; }
    }
}
