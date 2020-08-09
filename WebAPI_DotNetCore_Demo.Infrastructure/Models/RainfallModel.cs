using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebAPI_DotNetCore_Demo.Infrastructure.Models
{
    public class RainfallModel
    {
        // Used by Newtonsoft
        //[JsonProperty("api_info")]
        // Used by System.Net.Http.Json
        [JsonPropertyName("api_info")]
        public ApiInfoModel ApiInfo { get; set; }

        public MetadataModel Metadata { get; set; }
        public List<ItemModel> Items { get; set; }
    }
}
