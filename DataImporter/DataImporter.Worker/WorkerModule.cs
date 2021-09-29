using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

    }
}
