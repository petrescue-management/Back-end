using Newtonsoft.Json;
using PetRescue.Data.Models;
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
        public DateTime? DoB { get; set; }
        [JsonProperty("gender")]
        public int? Gender { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("imgUrl")]
        public string ImgUrl { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CenterProfileViewModel Center { get; set; }

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
        public DateTime? DoB { get; set; }
        [JsonProperty("gender")]
        public int? Gender { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("imgUrl")]
        public string ImgUrl { get; set; }
    }
    public class UserProfileViewModel
    {
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? DoB { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public string ImgUrl { get; set; }
    }
    public class UserProfileViewModel2
    {
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? DoB { get; set; }
        public int? Gender { get; set; }
        public string Phone { get; set; }
        public string ImgUrl { get; set; }
        public DateTime? DateStarted { get; set; }

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
        public Guid? CenterId { get; set; }
    }
    public class UserCreateByAppModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("status")]
        public int? Status { get; set; }

    }
    public class UserLoginBySysadminModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("deviceToken")]
        public string DeviceToken { get; set; }
        [JsonProperty("applicationName")]
        public string ApplicationName { get; set; }
    }
    public class UserUpdateModel
    {
        [JsonProperty("centerId")]
        public Guid CenterId { get; set; }
    }
    public class UserUpdateVolunteerStatus
    {
        [JsonProperty("status")]
        public int? Status { get; set; }
    }
    public class RemoveRoleVolunteerModel
    {
        public Guid UserId { get; set; }
        public string Description { get; set; }
    }
    public class UserModel
    {
        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? Dob { get; set; }
        public string Phone { get; set; }
        public int? Gender { get; set; }
        public string ImageUrl { get; set; }

    }
    public class UserLoginModel 
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("deviceToken")]
        public string DeviceToken { get; set; }
        [JsonProperty("applicationName")]
        public string ApplicationName { get; set; }
    }
    public class JWTReturnModel
    {
        public object Jwt { get; set; }
        public NotificationToken NotificationToken { get; set; }

    }
    public class CreateVolunteerModel
    {
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime Dob { get; set; }
        public byte Gender { get; set; }
        public string Phone { get; set; }
    }
    public class ChangeStatusModel
    {
        public Guid UserId { get; set; }
        public int? Status { get; set; }
    }
    public class UserLocation 
    {
        public double? Lat { get; set; }
        public double? Long { get; set; }
    }
    public class UserLocationModel
    {
        public Guid UserId { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
    }
    public class UserDistanceModel : IComparable<UserDistanceModel>
    {
        public double Value { get; set; }
        public Guid UserId { get; set; }
        public int CompareTo(UserDistanceModel model)
        {
            if (model == null) return 1;
            else
            {
                return this.Value.CompareTo(model.Value);
            }
        }
    }


}
