using Autofac;
using DataImporter.Worker.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Worker
{
    public class WorkerModule : Module
    {
        private string _connectionString;
        private string _migrationAssemblyName;
        private IConfiguration _configuration;

        public WorkerModule(string connectionString, string migrationAssemblyName, IConfiguration configuration)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TaskManagementService>().As<ITaskManagementService>()
                .InstancePerLifetimeScope();


            base.Load(builder);
        }
    }
}
