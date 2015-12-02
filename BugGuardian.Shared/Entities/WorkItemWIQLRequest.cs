using Newtonsoft.Json;

namespace DBTek.BugGuardian.Entities
{
    internal class WorkItemWIQLRequest : APIRequest
    {
        [JsonProperty(PropertyName = "query")]
        public string Query { get; set; }
    }
}
