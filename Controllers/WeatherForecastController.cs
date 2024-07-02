using cattoapi.CustomResponse;
using cattoapi.Interfaces.EmailServices;
using Microsoft.AspNetCore.Mvc;

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
        private readonly IEmailServicesRepo _emailServicesRepo;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IEmailServicesRepo emailServicesRepo)
        {
            _logger = logger;
            _emailServicesRepo = emailServicesRepo;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Test()
        {
            CustomResponse<bool> customResponse = await _emailServicesRepo.SendVerificationEmail(1);

            return StatusCode(customResponse.responseCode, customResponse);
        }
    }
}
