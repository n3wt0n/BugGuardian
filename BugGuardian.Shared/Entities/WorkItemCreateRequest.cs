using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DBTek.BugGuardian.Entities
{
    public class WorkItemCreateRequest : VSORequest
    {
        [JsonProperty(PropertyName = "op")]
        [JsonConverter(typeof(StringEnumConverter))]
        public WITOperationType Operation { get; set; }

        [JsonProperty(PropertyName = "path")]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

    }
}
