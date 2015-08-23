using Newtonsoft.Json;

namespace DBTek.BugGuardian.Entities
{
    internal class WorkItemWIQLRequest : VSORequest
    {
        [JsonProperty(PropertyName = "query")]
        public string Query { get; set; }
    }
}
