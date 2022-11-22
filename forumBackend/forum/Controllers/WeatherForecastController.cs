using forum;
using forum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace test2.Controllers
{
    [ApiController]
    //[Route("/")]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ForumContext _context;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, ForumContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult  Get()
            
        {
            var abc = _context.Articles;
            return StatusCode(200, abc);
        }
    }
}