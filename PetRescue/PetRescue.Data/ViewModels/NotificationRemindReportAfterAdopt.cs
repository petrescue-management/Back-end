using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRescue.Data.ViewModels
{
    public class NotificationRemindReportAfterAdopt
    {
        [JsonProperty("AdoptedAt")]
        public DateTime? AdoptedAt { get; set; }

        [JsonProperty("OwnerId")]
        public Guid OwnerId { get; set; }

        [JsonProperty("Path")]
        public string Path { get; set; }
    }
}
