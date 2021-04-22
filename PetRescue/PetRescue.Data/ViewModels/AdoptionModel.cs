using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class AdoptionModel
    {
        public Guid AdoptionRegistrationId { get; set; }
        public UserModel Owner { get; set; }
        public PetProfileModel PetProfile { get; set; }
        public int AdoptionStatus { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
    public class AdoptionViewModel
    {
        public Guid AdoptionRegistrationId { get; set; }
        public UserModel Owner { get; set; }
        public int AdoptionStatus { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Job { get; set; }
        public string PetName { get; set; }
        public string PetImgUrl { get; set; }
        public string PetBreedName { get; set; }
        public string PetTypeName { get; set; }
        public string PetColorName { get; set; }
        public List<PetTrackingViewModel> PetTrackings { get; set; }
    }
    public class AdoptionViewModelMobile
    {
        public Guid AdoptionRegistrationId { get; set; }
        public UserModel Owner { get; set; }
        public int AdoptionStatus { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Job { get; set; }
        public string PetName { get; set; }
        public string PetImgUrl { get; set; }
        public string PetBreedName { get; set; }
        public string PetFurColorName { get; set; }
        public int Gender { get; set; }
        public int Age { get; set; }
        public string CenterName { get; set; }
        public string CenterAddress { get; set; }
        public Guid PetProfileId { get; set; }
    }
    public class AdoptionCreateModel
    {
        public Guid AdoptionRegistrationFormId { get; set; }
    }
    public class AdoptionViewModelWeb
    {
        public Guid AdoptionRegistrationId { get; set; }
        public UserModel Owner { get; set; }
        public int AdoptionStatus { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Job { get; set; }
        public string PetName { get; set; }
        public string PetImgUrl { get; set; }
        public string PetBreedName { get; set; }
        public string PetTypeName { get; set; }
        public string PetColorName { get; set; }
        public List<PetTrackingViewModel> PetTrackings { get; set; }
        public List<AdoptionReportTrackingViewModel> AdoptionReports { get; set; }
    }

}
