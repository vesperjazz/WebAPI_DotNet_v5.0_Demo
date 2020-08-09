using System;
using System.Collections.Generic;

namespace WebAPI_DotNetCore_Demo.Application.DataTransferObjects.Weather
{
    public sealed class ItemDto
    {
        public DateTime Timestamp { get; set; }
        public List<ReadingDto> Readings { get; set; }
    }
}
