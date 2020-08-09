using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebAPI_DotNetCore_Demo.Infrastructure.Models
{
    public class RelativeHumidityModel
    {
        [JsonPropertyName("api_info")]
        public ApiInfoModel ApiInfo { get; set; }

        public MetadataModel Metadata { get; set; }
        public List<ItemModel> Items { get; set; }
    }
}
