using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataImporter.Importing;
using DataImporter.Importing.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Worker
{
    public class Program
    {
        private static string _migrationAssemblyName;
        private static IConfiguration _configuration;

        public static void Main(string[] args)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            _migrationAssemblyName = typeof(Program).Assembly.FullName;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();
            try
            {
                Log.Information("Application Starting up");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .UseSerilog()
                .ConfigureContainer<ContainerBuilder>((hostContext, builder) =>
                {
                    var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

                    builder.RegisterModule(new ImportingModule(connectionString,
                         _migrationAssemblyName));
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

                    services.AddHostedService<Worker>();
                    services.AddDbContext<ImportingContext>(options =>
                         options.UseSqlServer(connectionString, b =>
                         b.MigrationsAssembly(_migrationAssemblyName)));
                });
    }
}
