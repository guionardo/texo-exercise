using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using texo.api.Responses;
using texo.data.Interfaces;

namespace texo.api.Controllers
{
    [ApiController]
    [Route("prizes")]
    public class PrizesController : Controller
    {
        private readonly IProducerService _producerService;

        public PrizesController(IProducerService producerService)
        {
            _producerService = producerService;
        }

        [HttpGet("intervals")]
        public async Task<ActionResult<WinningProducers>> GetPrizeIntervals()
        {
            var (minWinners,maxWinners) = await _producerService.GetFirstAndLastWinProducers();
            
            return Ok(new WinningProducers
            {
                Min = minWinners,
                Max = maxWinners
            });
        }
    }
}