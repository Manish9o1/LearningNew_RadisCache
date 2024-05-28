using Microsoft.AspNetCore.Mvc;
using RappidMQ.IRepo;
using RappidMQ.Models;

namespace RappidMQ.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IRepoInterFace _repo;
       
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IRepoInterFace repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("PostProduct")]
        public async Task<ActionResult> PostProduct([FromBody] Product product)
        {
            var result = await _repo.PostProduct(product);
            return Ok(result);
        }
        [HttpGet("GetProduct")]
        public async Task<ActionResult> GetProduct()
        {
            var result = await _repo.GetAllProduct();
            return Ok(result);
        }
        [HttpGet("GetProductFromCache/{productId}")]
        public async Task<ActionResult> GetProductFromCache(int productId)
        {
            var result = await _repo.GetProductFromCache(productId);
            return Ok(result);
        }
    }
}
