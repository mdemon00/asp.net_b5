using Autofac;
using InventorySystem.Stocking.Contexts;
using InventorySystem.Stocking.Repositories;
using InventorySystem.Stocking.Services;
using InventorySystem.Stocking.UnitOfWorks;

namespace InventorySystem.Stocking
{
    public class StockingModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public StockingModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StockingContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<StockingContext>().As<IStockingContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<StockingUnitOfWork>().As<IStockingUnitOfWork>()
            .InstancePerLifetimeScope();
            base.Load(builder);   
            
            builder.RegisterType<ProductRepository>().As<IProductRepository>()
            .InstancePerLifetimeScope();
            base.Load(builder);

            builder.RegisterType<ProductService>().As<IProductService>()
            .InstancePerLifetimeScope();
            base.Load(builder);

            builder.RegisterType<StockRepository>().As<IStockRepository>()
            .InstancePerLifetimeScope();
            base.Load(builder);

            builder.RegisterType<StockService>().As<IStockService>()
            .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
