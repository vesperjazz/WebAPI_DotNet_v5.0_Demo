using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Weather;
using WebAPI_DotNetCore_Demo.Application.Infrastructure;

namespace WebAPI_DotNetCore_Demo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
        }

        [HttpGet("rainfall")]
        public async Task<ActionResult<RainfallDto>> GetRainFallAsync(CancellationToken cancellationToken)
        {
            return await _weatherService.GetRainFallAsync(cancellationToken);
        }

        [HttpGet("relativehumidity")]
        public async Task<ActionResult<RelativeHumidityDto>> GetRelativeHumidityAsync(CancellationToken cancellationToken)
        {
            return await _weatherService.GetRelativeHumidityAsync(cancellationToken);
        }
    }
}
