using Newtonsoft.Json;

namespace DBTek.BugGuardian.Entities
{
    internal class WorkItemWIQLRequest : VSTSRequest
    {
        [JsonProperty(PropertyName = "query")]
        public string Query { get; set; }
    }
}
