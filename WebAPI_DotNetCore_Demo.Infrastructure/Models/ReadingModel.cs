using System.Text.Json.Serialization;

namespace WebAPI_DotNetCore_Demo.Infrastructure.Models
{
    public class ReadingModel
    {
        [JsonPropertyName("station_id")]
        public string StationID { get; set; }

        public double Value { get; set; }
    }
}
