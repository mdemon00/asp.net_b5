using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Email;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Question_One
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
            .Build();

            string connectionString = "Server=DESKTOP-9BILDI2\\SQLEXPRESS;Database=aspnetB5;User Id = sa; Password=;";

            var networkCredential = new NetworkCredential { UserName = "mdemon0064@gmail.com", Password = "" };

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                //log in database
                .WriteTo.MSSqlServer(connectionString,
                sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs" }
                , null, null, LogEventLevel.Information, null, null, null, null)
                //log in email
                .WriteTo.Email(new EmailConnectionInfo
                {
                    FromEmail = "mdemon0064@gmail.com",
                    ToEmail = "md.emon.au@gmail.com",
                    MailServer = "smtp.gmail.com",
                    NetworkCredentials = networkCredential,
                    EnableSsl = true,
                    Port = 465,
                    EmailSubject = "Test Mail"
                },
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}",
                batchPostingLimit: 10
                , restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information
                )
                .CreateLogger();

            Log.Information("This is a test..");
            Log.CloseAndFlush();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://*:80");
                });
    }
}
