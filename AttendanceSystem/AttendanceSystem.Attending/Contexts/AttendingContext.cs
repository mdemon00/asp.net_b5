using AttendanceSystem.Attending.Entites;
using Microsoft.EntityFrameworkCore;

namespace AttendanceSystem.Attending.Contexts
{
    public class AttendingContext : DbContext, IAttendingContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public AttendingContext(string connectionString, string migrationAssemblyName)
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

        public DbSet<Student> Students { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
    }
}
