using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces.BlackListTokens;
using cattoapi.Interfaces.EmailServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static cattoapi.utlities.Utlities;

namespace cattoapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IBlackListTokensRepo _blackListTokensRepo;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IBlackListTokensRepo blackListTokensRepo)
        {
            _logger = logger;
            _blackListTokensRepo = blackListTokensRepo;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        
        public async Task<IActionResult> Test()
        {
        
            return Ok();
        }
    }
}
