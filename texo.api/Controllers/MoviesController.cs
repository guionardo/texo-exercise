using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace texo.api.Controllers
{
    [ApiController]
    [Route("prizes")]
    public class MoviesController : Controller
    {
        [HttpGet("intervals")]
        public async Task<IActionResult> GetPrizeIntervals()
        {
            return Ok("OK");
        }
    }
}