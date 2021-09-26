using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using texo.api.Controllers;
using texo.data.DependencyInjection;
using texo.data.Interfaces;
using Xunit;

namespace texo.tests.api.Controllers
{
    [ExcludeFromCodeCoverage]
    public class Prizes
    {
        [Fact]
        public async Task TestPrizesController()
        {
            const string testingDatabase = "texo.testing.sqlite";
            if (File.Exists(testingDatabase))
            {
                File.Delete(testingDatabase);
            }

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ConnectionStrings:Default", $"Data Source={testingDatabase}" },
                    { "ConnectionStrings:source_file", "movielist.csv" }
                })
                .Build();

            var services = new ServiceCollection();
            services
                .AddSingleton<IConfiguration>(configuration)
                .AddLogging()
                .AddDataServices();


            var provider = services.BuildServiceProvider();
            var databaseBootstrap = provider.GetService<IDatabaseBootstrap>();
            databaseBootstrap.Setup();
            var producerService = provider.GetService<IProducerService>();
            var aggregatorService = provider.GetService<IAggregatorService>();
            await aggregatorService.LoadDataFromCsv();

            var prizesControler = new PrizesController(producerService);

            var result = await prizesControler.GetPrizeIntervals();
            Assert.Equal(200, (result.Result as OkObjectResult).StatusCode);
            // Assert.Equal(1, result.Value.Min.First().Interval);
        }
    }
}