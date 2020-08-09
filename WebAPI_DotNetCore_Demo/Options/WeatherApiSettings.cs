using System;

namespace WebAPI_DotNetCore_Demo.Options
{
    public sealed class WeatherApiSettings
    {
        public TimeSpan Timeout { get; set; }
        public string BaseAddress { get; set; }
    }
}
