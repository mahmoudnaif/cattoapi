using cattoapi.CustomResponse;
using cattoapi.DTOS;
using cattoapi.Interfaces.BlackListTokens;
using cattoapi.Interfaces.EmailServices;
using cattoapi.Repos;
using cattoapi.utlities;
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
        private readonly TokensRepo _tokensRepo;

        public WeatherForecastController(TokensRepo tokensRepo)
        {
            _tokensRepo = tokensRepo;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        
        public async Task<IActionResult> Test([FromQuery] string token)
        {
            return Ok(await _tokensRepo.IsTokenValid(token));
        }
    }
}
