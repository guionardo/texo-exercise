using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using texo.data.DependencyInjection;
using texo.data.Interfaces;

namespace texo.tests.api.Controllers
{
    [ExcludeFromCodeCoverage]
    public static class ServicesAux
    {
        private const string TestingDatabase = "texo.testing.sqlite";

        private static bool _databaseSetupDone;
        private static IServiceProvider _serviceProvider;

        static ServicesAux()
        {
            if (File.Exists(TestingDatabase))
            {
                File.Delete(TestingDatabase);
            }
        }

        public static IServiceProvider GetServiceProvider()
        {
            if (_serviceProvider is not null)
            {
                return _serviceProvider;
            }

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ConnectionStrings:Default", $"Data Source={TestingDatabase}" },
                    { "ConnectionStrings:source_file", "movielist.csv" }
                })
                .Build();

            var services = new ServiceCollection();
            services
                .AddSingleton<IConfiguration>(configuration)
                .AddLogging()
                .AddDataServices();
            _serviceProvider = services.BuildServiceProvider();
            return _serviceProvider;
        }

        public static void SetupDatabase()
        {
            if (_databaseSetupDone)
                return;
            var databaseBootstrap = GetServiceProvider().GetService<IDatabaseBootstrap>();
            databaseBootstrap.Setup();
            var aggregatorService = GetServiceProvider().GetService<IAggregatorService>();
            aggregatorService.LoadDataFromCsv().Wait();
            _databaseSetupDone = true;
        }
    }
}