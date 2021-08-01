using Microsoft.EntityFrameworkCore;
using TicketBookingSystem.Booking.Entites;

namespace TicketBookingSystem.Booking.Contexts
{
    public class BookingContext : DbContext, IBookingContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public BookingContext(string connectionString, string migrationAssemblyName)
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

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
