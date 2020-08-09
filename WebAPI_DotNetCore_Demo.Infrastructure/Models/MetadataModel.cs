using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebAPI_DotNetCore_Demo.Infrastructure.Models
{
    public class MetadataModel
    {
        [JsonPropertyName("reading_type")]
        public string ReadingType { get; set; }

        [JsonPropertyName("reading_unit")]
        public string ReadingUnit { get; set; }

        public List<StationModel> Stations { get; set; }
    }
}
