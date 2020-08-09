using System.Text.Json.Serialization;

namespace WebAPI_DotNetCore_Demo.Infrastructure.Models
{
    public class StationModel
    {
        public string Id { get; set; }

        [JsonPropertyName("device_id")]
        public string DeviceID { get; set; }

        public string Name { get; set; }
        public LocationModel Location { get; set; }
    }
}
