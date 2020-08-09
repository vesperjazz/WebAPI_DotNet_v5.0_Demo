using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Weather;

namespace WebAPI_DotNetCore_Demo.Application.Infrastructure
{
    public interface IWeatherService
    {
        Task<RainfallDto> GetRainFallAsync(CancellationToken cancellationToken = default);
        Task<RelativeHumidityDto> GetRelativeHumidityAsync(CancellationToken cancellationToken = default);
    }
}
