﻿using DataImporter.Importing.Entities;
using DataImporter.Membership.Entities;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers",t => t.ExcludeFromMigrations())
                .HasMany<Group>()
                .WithOne(t => t.ApplicationUser);

            modelBuilder.Entity<ApplicationUser>()
                .ToTable("AspNetUsers", t => t.ExcludeFromMigrations())
                .HasMany<History>()
                .WithOne(t => t.ApplicationUser);

            modelBuilder.Entity<History>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Histories)
                .HasForeignKey(s => s.GroupId)
                .OnDelete(DeleteBehavior.NoAction);
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Row> Rows { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<History> History { get; set; }
    }
}
