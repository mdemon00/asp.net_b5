using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.ResolveAnything;
using DataImporter.Importing;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Services.Mail;
using DataImporter.Importing.Settings;
using DataImporter.Membership;
using DataImporter.Membership.Contexts;
using DataImporter.Membership.Entities;
using DataImporter.Membership.Services;
using DataImporter.Worker.Models;
using Microsoft.AspNetCore.Identity;
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
                .AddJsonFile("appsettings.Development.json", false)
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

                    builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

                    builder.RegisterModule(new ImportingModule(connectionString,
                         _migrationAssemblyName));
                    builder.RegisterModule(new MembershipModule(connectionString,
                         _migrationAssemblyName));
                    builder.RegisterModule(new WorkerModule(connectionString,
                         _migrationAssemblyName, _configuration));
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

                    services.AddHostedService<Worker>();
                    services.AddDbContext<ImportingContext>(options =>
                         options.UseSqlServer(connectionString, b =>
                         b.MigrationsAssembly(_migrationAssemblyName)));

                    services.AddHttpContextAccessor();
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(connectionString, b =>
                        b.MigrationsAssembly(_migrationAssemblyName)));


                    services.AddDbContext<ImportingContext>(options =>
                        options.UseSqlServer(connectionString, b =>
                        b.MigrationsAssembly(_migrationAssemblyName)));

                    // Identity customization started here
                    services
                        .AddIdentity<ApplicationUser, Role>()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddUserManager<UserManager>()
                        .AddRoleManager<RoleManager>()
                        .AddSignInManager<SignInManager>()
                        .AddDefaultUI()
                        .AddDefaultTokenProviders();

                    services.Configure<IdentityOptions>(options =>
                    {
                        // Password settings.
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = true;
                        options.Password.RequireNonAlphanumeric = true;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequiredUniqueChars = 1;

                        // Lockout settings.
                        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                        options.Lockout.MaxFailedAccessAttempts = 5;
                        options.Lockout.AllowedForNewUsers = true;

                        // User settings.
                        options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                        options.User.RequireUniqueEmail = false;
                    });

                    services.Configure<WorkerSettingsModel>(_configuration.GetSection("DataImporter"));
                    services.AddTransient<IMailService, MailService>();
                    services.Configure<MailSettings>(_configuration.GetSection("MailSettings"));

                });
    }
}
