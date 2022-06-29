using ClassGenerator_BETA_.Interfaces.Repository;
using ClassGenerator_BETA_.Interfaces.Service;
using ClassGenerator_BETA_.Service;
using Client.API.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace ClassGenerator_BETA_
{
    class Program
    {
        public static IConfiguration _configuration;
        public static string _connectionString { get; private set; }
        static void Main(string[] args)
        {
            var host = CreateDefaultBuilder().Build();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            var workerInstance = provider.GetRequiredService<IGenerator>();

            var logger = provider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogDebug("Starting application");

            workerInstance.CreateClass();
            host.Run();

            logger.LogDebug("All done!");
        }
        static IHostBuilder CreateDefaultBuilder()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            _connectionString = _configuration.GetConnectionString("DefaultConnection");

            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(services =>
                {
                    services
                     .AddSingleton(LoggerFactory.Create(builder =>
                     {
                         builder
                             .AddSerilog(dispose: true);
                     }))
                    .AddSingleton(_configuration)
                    .AddScoped<IGenerator, Generator>()
                    .AddScoped<IGenericRepository, GenericRepository>()
                    .AddScoped<IGenericService, GenericService>()
                    .AddScoped<ITranslateTableNames, TranslateTableNames>()
                    .AddScoped<IInterfaceGenerator, InterfaceGenerator>()
                    .AddScoped<IClassContentGenerator, ClassContentGenerator>()
                    .AddScoped<IControllerGenerator, ControllerGenerator>()
                    .AddScoped<IDependencyInjectionGenerator, DependencyInjectionGenerator>()
                    .AddScoped<IGenericClassGenerator, GenericClassGenerator>()
                    .AddScoped<IAPIGatewayControllerGenerator, APIGatewayControllerGenerator>()
                    .AddScoped<IAPIGatewayMethodControllerGenerator, APIGatewayMethodControllerGenerator>()
                    .AddScoped<IGenericClassGenerator, GenericClassGenerator>()
                    .AddLogging(configure => configure.AddConsole())
                    .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
                    .BuildServiceProvider();
                   
                });
        }
    }
}
