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
    public class AddNewRoleModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("centerId")]
        public Guid CenterId { get; set; }
        [JsonProperty("roleName")]
        public string RoleName { get; set; }
        [JsonProperty("insertBy")]
        public Guid InsertBy { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime DoB { get; set; }
        public byte Gender { get; set; }
        public string Phone { get; set; }
    }
    public class RemoveVolunteerRoleModel
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
        [JsonProperty("centerId")]
        public Guid CenterId { get; set; }
        [JsonProperty("insertBy")]
        public Guid InsertBy { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
    }
    public class UserRoleUpdateEntityModel
    {
        public Guid UpdateBy { get; set; }
        public bool IsActive { get; set; }
    }
}
