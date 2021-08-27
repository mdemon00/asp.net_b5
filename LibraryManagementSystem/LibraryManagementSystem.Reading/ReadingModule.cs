using Autofac;
using LibraryManagementSystem.Reading.Contexts;
using LibraryManagementSystem.Reading.Repositories;
using LibraryManagementSystem.Reading.Services;
using LibraryManagementSystem.Reading.UnitOfWorks;


namespace LibraryManagementSystem.Reading
{
    public class ReadingModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ReadingModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ReadingContext>().AsSelf()
            .WithParameter("connectionString", _connectionString)
            .WithParameter("migrationAssemblyName", _migrationAssemblyName)
            .InstancePerLifetimeScope();

            builder.RegisterType<ReadingContext>().As<IReadingContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<BookRepository>().As<IBookRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BookService>().As<IBookService>()
                 .InstancePerLifetimeScope();

            builder.RegisterType<AuthorRepository>().As<IAuthorRepository>()
                 .InstancePerLifetimeScope();

            builder.RegisterType<AuthorService>().As<IAuthorService>()
                 .InstancePerLifetimeScope();

            builder.RegisterType<ReadingUnitOfWork>().As<IReadingUnitOfWork>()
                .InstancePerLifetimeScope();    
            

            base.Load(builder);
        }
    }
}
