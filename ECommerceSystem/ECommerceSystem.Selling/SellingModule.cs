using Autofac;
using ECommerceSystem.Selling.Contexts;
using ECommerceSystem.Selling.Repositories;
using ECommerceSystem.Selling.Services;
using ECommerceSystem.Selling.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSystem.Selling
{
    public class SellingModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public SellingModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SellingContext>().AsSelf()
            .WithParameter("connectionString", _connectionString)
            .WithParameter("migrationAssemblyName", _migrationAssemblyName)
            .InstancePerLifetimeScope();

            builder.RegisterType<SellingContext>().As<ISellingContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<ProductRepository>().As<IProductRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SellingUnitOfWork>().As<ISellingUnitOfWork>()
                .InstancePerLifetimeScope();    
            
            builder.RegisterType<ProductService>().As<IProductService>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
