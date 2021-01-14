using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
   
    public class UserDetailModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("roles")]
        public string[] Roles { get; set; }
        [JsonProperty("full-name")]
        public string FullName { get; set; }

    }

}
