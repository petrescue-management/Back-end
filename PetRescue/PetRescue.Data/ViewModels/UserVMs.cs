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
    public class UserProfileUpdateModel
    {
        [JsonProperty("user-id")]
        public Guid UserId { get; set; }
        [JsonProperty("last-name")]
        public string LastName { get; set; }
        [JsonProperty("first-name")]
        public string FirstName { get; set; }
        [JsonProperty("dob")]
        public DateTime DoB { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("gender")]
        public bool Gender { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
    public class UserUpdateCenterModel
    {
        [JsonProperty("user-id")]
        public Guid UserId { get; set; }
        [JsonProperty("center-id")]
        public Guid CenterId { get; set; }
    }

}
