using Newtonsoft.Json;
using System.Collections.Generic;

namespace DBTek.BugGuardian.Entities
{
    public class WorkItemData
    {
        [JsonProperty(PropertyName = "id")]
        internal int ID { get; set; }

        internal List<History> History { get; set; }

        internal string Title
            => Fields?.Title;

        [JsonProperty(PropertyName = "fields")]
        internal Fields Fields { get; set; }
    }

    internal class Fields
    {
        [JsonProperty(PropertyName = "System.Title")]
        public string Title { get; set; }
    }

    internal class History
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
