using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class NotificationTokenCreateModel
    {
        [JsonProperty("deviceToken")]
        public string DeviceToken { get; set; }
        [JsonProperty("applicationName")]
        public string ApplicationName { get; set; }
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
    }
    public class NotificationTokenUpdateModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("deviceToken")]
        public string DeviceToken { get; set; }
    }
}
