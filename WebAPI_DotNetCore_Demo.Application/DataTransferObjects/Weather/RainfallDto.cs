using System.Collections.Generic;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Weather
{
    public sealed class RainfallDto
    {
        public string Status { get; set; }
        public string ReadingType { get; set; }
        public string ReadingUnit { get; set; }
        public List<StationDto> Stations { get; set; }
        public List<ItemDto> Items { get; set; }
    }
}
