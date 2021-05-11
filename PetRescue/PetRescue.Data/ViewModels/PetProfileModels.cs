using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class PetProfileModel
    {
        public Guid PetProfileId { get; set; }
        public Guid? PetDocumentId { get; set; }
        public string PetName { get; set; }
        public int? PetGender { get; set; }
        public int? PetAge { get; set; }
        public Guid? PetBreedId { get; set; }
        public string PetBreedName { get; set; }
        public Guid? PetFurColorId { get; set; }
        public string PetFurColorName { get; set; }
        public string PetImgUrl { get; set; }
        public int? PetStatus { get; set; }
        public string Description { get; set; }
        public Guid? CenterId { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public CenterProfileViewModel CenterProfile { get; set; }
        public PetTypeUpdateModel PetType { get; set; }
    }
    public class PetProfileModel2
    {
        public Guid PetProfileId { get; set; }
        public Guid? PetDocumentId { get; set; }
        public string PetName { get; set; }
        public int? PetGender { get; set; }
        public int? PetAge { get; set; }
        public Guid? PetBreedId { get; set; }
        public string PetBreedName { get; set; }
        public Guid? PetFurColorId { get; set; }
        public string PetFurColorName { get; set; }
        public string PetImgUrl { get; set; }
        public int? PetStatus { get; set; }
        public string Description { get; set; }
        public Guid? CenterId { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }
        public string CenterAddress { get; set; }
        public string CenterName { get; set; }
        public PetTypeUpdateModel PetType { get; set; }
    }
    public class PetProfileFilter
    {
        [JsonProperty("petDocumentId")]
        public Guid PetDocumentId { get; set; }
        [JsonProperty("centerId")]
        public Guid CenterId { get; set; }
        [JsonProperty("petStatus")]
        public int PetStatus { get; set; }
        [JsonProperty("petTypeName")]
        public string PetTypeName { get; set; }
        [JsonProperty("petBreedName")]
        public string PetBreedName { get; set; }
        [JsonProperty("petFurColorName")]
        public string PetFurColorName { get; set; }
        [JsonProperty("petAge")]
        public int PetAge { get; set; }
    }
    public class PetProfileModel3
    {
        public Guid PetProfileId { get; set; }
        public Guid? PetDocumentId { get; set; }
        public string PetName { get; set; }
        public int? PetGender { get; set; }
        public int? PetAge { get; set; }
        public Guid? PetBreedId { get; set; }
        public string PetBreedName { get; set; }
        public Guid? PetFurColorId { get; set; }
        public string PetFurColorName { get; set; }
        public string PetImgUrl { get; set; }
        public int? PetStatus { get; set; }
        public string Description { get; set; }
        public Guid? CenterId { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }
        public string CenterAddress { get; set; }
        public string CenterName { get; set; }

    }
    public class CreatePetProfileModel
    {
        public Guid PetDocumentId { get; set; }
        public string PetName { get; set; }
        public int PetGender { get; set; }
        public int PetAge { get; set; }
        public Guid PetBreedId { get; set; }
        public Guid PetFurColorId { get; set; }
        public string PetImgUrl { get; set; }
        public int PetStatus { get; set; }
        public string Description { get; set; }
    }
    public class UpdatePetProfileModel
    {
        public Guid? PetProfileId { get; set; }
        public Guid PetDocumentId { get; set; }
        public string PetName { get; set; }
        public int PetGender { get; set; }
        public int PetAge { get; set; }
        public Guid PetBreedId { get; set; }
        public Guid PetFurColorId { get; set; }
        public string PetImgUrl { get; set; }
        public int PetStatus { get; set; }
        public string Description { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class GetPetByTypeNameModel
    {
        public string TypeName { get; set; }
        public List<PetProfileModel3> Result { get; set; }
    }
    public class SearchPetProfileModel
    {
        public Guid PetTypeId { get; set; }
        public int PetGender { get; set; }
        public int PetAge { get; set; }
        public Guid PetBreedId { get; set; }
        public Guid PetFurColorId { get; set; }
        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int PetStatus { get; set; }


    }

    public class PetProfileMobile
    {
        public Guid? PetProfileId { get; set; }
        public Guid? PetDocumentId { get; set; }
        public string PetName { get; set; }
        public int? PetGender { get; set; }
        public int? PetAge { get; set; }
        public Guid? PetBreedId { get; set; }
        public string PetBreedName { get; set; }
        public Guid? PetFurColorId { get; set; }
        public string PetFurColorName { get; set; }
        public string PetImgUrl { get; set; }
        public int? PetStatus { get; set; }
        public string PetProfileDescription { get; set; }
        public Guid? CenterId { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }
        public string CenterAddress { get; set; }
        public string CenterName { get; set; }
    }

    public class PetAdoptionProfile
    {
        public Guid AdoptionRegistrationId { get; set; }
        public UserModel Owner { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Job { get; set; }
        public string PetName { get; set; }
        public string PetImgUrl { get; set; }
        public string PetBreedName { get; set; }
        public string PetFurColorName { get; set; }
        public int? Gender { get; set; }
        public int? Age { get; set; }
        public string CenterName { get; set; }
        public string CenterAddress { get; set; }
        public Guid PetProfileId { get; set; }
    }
    public class PetAdoptionViewModel
    {
        public Guid AdoptionRegistrationId { get; set; }
        public UserModel Owner { get; set; }
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
    public class PetAdoptionViewModelWeb
    {
        public Guid AdoptionRegistrationId { get; set; }
        public UserModel Owner { get; set; }
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
    public class PetAdoptionCreateModel
    {
        public Guid AdoptionRegistrationFormId { get; set; }
    }
}
