using Autofac;
using Microsoft.Extensions.Configuration;

namespace StockData.Worker
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

    }
}