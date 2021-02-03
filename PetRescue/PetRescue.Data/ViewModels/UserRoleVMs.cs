using Newtonsoft.Json;
using PetRescue.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class UserRoleUpdateModel
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        [JsonProperty("centerId")]
        public Guid CenterId { get; set; }
        
    }
}
