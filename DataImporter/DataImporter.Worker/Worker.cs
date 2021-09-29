using DataImporter.Importing.Services;
using DataImporter.Worker.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DataImporter.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private WorkerSettingsModel _settings;

        public Worker(ILogger<Worker> logger, IOptions<WorkerSettingsModel> settings)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            _logger = logger;
            _settings = settings.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                }
                catch(Exception ex)
                {
                    _logger.LogInformation("Error {ex} {time}", ex, DateTimeOffset.Now);
                }

                await Task.Delay(_settings.Worker_Delay_Time, stoppingToken);
            }
        }
    }
}
