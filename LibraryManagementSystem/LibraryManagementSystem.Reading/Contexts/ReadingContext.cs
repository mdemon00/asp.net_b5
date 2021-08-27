using LibraryManagementSystem.Reading.Entites;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagementSystem.Reading.Contexts
{
    public class ReadingContext : DbContext, IReadingContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ReadingContext(string connectionString, string migrationAssemblyName)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Book>()
                .HasMany(c => c.Authors)
                .WithOne(t => t.Book);

            modelBuilder.Entity<BookAuthors>()
                .HasKey(cs => new { cs.BookId, cs.AuthorId });

            modelBuilder.Entity<BookAuthors>()
                .HasOne(cs => cs.Book)
                .WithMany(c => c.RentedBooks)
                .HasForeignKey(cs => cs.BookId);

            modelBuilder.Entity<BookAuthors>()
                .HasOne(cs => cs.Author)
                .WithMany(s => s.RentedBooks)
                .HasForeignKey(cs => cs.AuthorId);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}

