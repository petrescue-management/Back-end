using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRescue.Data.ViewModels
{
    public class NotificationToVolunteers
    {
        [JsonProperty("FinderFormId")]
        public Guid FinderFormId { get; set; }

        [JsonProperty("InsertedAt")]
        public DateTime InsertedAt { get; set; }

        [JsonProperty("InsertedBy")]
        public Guid InsertedBy { get; set; }

        [JsonProperty("Path")]
        public string Path { get; set; }
    }
}
