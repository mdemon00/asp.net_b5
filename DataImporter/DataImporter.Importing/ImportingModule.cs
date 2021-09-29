using Autofac;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Repositories;
using DataImporter.Importing.Services;
using DataImporter.Importing.UnitOfWorks;

namespace DataImporter.Importing
{
    public class ImportingModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public ImportingModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImportingContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<ImportingContext>().As<IImportingContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();


            builder.RegisterType<GroupRepository>().As<IGroupRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ColumnRepository>().As<IColumnRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RowRepository>().As<IRowRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CellRepository>().As<ICellRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<HistoryRepository>().As<IHistoryRepository>()
                .InstancePerLifetimeScope();


            builder.RegisterType<GroupService>().As<IGroupService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<ColumnService>().As<IColumnService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<RowService>().As<IRowService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ExcelService>().As<IExcelService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<CellService>().As<ICellService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<HistoryService>().As<IHistoryService>()
                .InstancePerLifetimeScope();


            builder.RegisterType<ImportingUnitOfWork>().As<IImportingUnitOfWork>()
                 .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
