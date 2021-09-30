using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using texo.api.Controllers;
using texo.data.Interfaces;
using Xunit;

namespace texo.tests.api.Controllers
{
    [ExcludeFromCodeCoverage]
    public class Prizes
    {
     //   [Fact]
        public async Task TestPrizesController()
        {
            var provider = ServicesAux.GetServiceProvider();
            ServicesAux.SetupDatabase();
            
            var producerService = provider.GetService<IProducerService>();
            var prizesControler = new PrizesController(producerService);

            var result = await prizesControler.GetPrizeIntervals();
            Assert.Equal(200, (result.Result as OkObjectResult).StatusCode);
            // Assert.Equal(1, result.Value.Min.First().Interval);
        }
    }
}