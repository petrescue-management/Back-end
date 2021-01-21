using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class UserRoleUpdateModel
    {
        [JsonProperty("user-id")]
        public Guid UserId { get; set; }
        [JsonProperty("role-name")]
        public string RoleName { get; set; }
        [JsonProperty("center-id")]
        public Guid CenterId { get; set; }
        
    }
}
