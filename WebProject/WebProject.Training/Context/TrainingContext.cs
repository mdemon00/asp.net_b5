using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProject.Training.Entities;

namespace WebProject.Training.Context
{
    public class TrainingContext : DbContext, ITrainingContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public TrainingContext(string connectionString, string migrationAssemblyName)
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
            // one to many relationship
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Topics)
                .WithOne(t => t.Course);

            modelBuilder.Entity<CourseStudents>()
                .HasKey(cs => new { cs.CourseId, cs.StudentId});

            modelBuilder.Entity<CourseStudents>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.EnrolledStudents)
                .HasForeignKey(cs => cs.CourseId);

            modelBuilder.Entity<CourseStudents>()
                .HasOne(cs => cs.Student)
                .WithMany(c => c.EnrolledCourses)
                .HasForeignKey(cs => cs.StudentId);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Student> Students { get; set; }

    }
}
