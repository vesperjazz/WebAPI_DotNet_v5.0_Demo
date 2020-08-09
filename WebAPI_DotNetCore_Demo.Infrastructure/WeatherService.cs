using AutoMapper;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Weather;
using WebAPI_DotNetCore_Demo.Application.Infrastructure;
using WebAPI_DotNetCore_Demo.Infrastructure.Models;

namespace WebAPI_DotNetCore_Demo.Infrastructure
{
    public class WeatherService : IWeatherService
    {
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        public WeatherService(IMapper mapper, HttpClient httpClient)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            // The HttpClient is injected here is using the Typed client approach.
            // Its lifetime is managed by the DI container and should not be manually disposed.
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<RainfallDto> GetRainFallAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("environment/rainfall");

            // Install-Package System.Net.Http.Json
            var model = await response.Content.ReadFromJsonAsync<RainfallModel>(
                cancellationToken: cancellationToken);

            return _mapper.Map<RainfallDto>(model);
        }

        public async Task<RelativeHumidityDto> GetRelativeHumidityAsync(CancellationToken cancellationToken = default)
        {
            var response = await _httpClient.GetAsync("environment/relative-humidity");
            var model = await response.Content.ReadFromJsonAsync<RelativeHumidityModel>(
                cancellationToken: cancellationToken);

            return _mapper.Map<RelativeHumidityDto>(model);
        }
    }
}
