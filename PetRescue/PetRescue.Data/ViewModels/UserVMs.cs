﻿using Newtonsoft.Json;
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
        [JsonProperty("centerId")]
        public Guid? CenterId { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("dob")]
        public DateTime DoB { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("gender")]
        public byte Gender { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("imgUrl")]
        public string ImgUrl { get; set; }

    }
    public class UserProfileUpdateModel
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("dob")]
        public DateTime DoB { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("gender")]
        public byte Gender { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("imgUrl")]
        public string ImgUrl { get; set; }
    }
    public class UserProfileViewModel
    {
        
    }
    public class UserUpdateCenterModel
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }
        [JsonProperty("centerId")]
        public Guid CenterId { get; set; }
    }
    public class UserCreateModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("centerId")]
        public Guid CenterId { get; set; }
        [JsonProperty("isBelongToCenter")]
        public bool IsBelongToCenter { get; set; }
    }
    public class UserCreateByAppModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }

}
