using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class PetModel
    {
        public Guid PetId { get; set; }
        public string PetName { get; set; }
        public int PetGender { get; set; }
        public int? PetAge { get; set; }
        public double Weight { get; set; }
        public string Description { get; set; }
        public Guid PetBreedId { get; set; }
        public string PetBreedName { get; set; }
        public Guid PetFurColorId { get; set; }
        public string PetFurColorName { get; set; }
        public bool IsVaccinated { get; set; }
        public bool IsSterilized { get; set; }
        public int PetStatus { get; set; }
        public Guid CenterId { get; set; }
    }
    public class PetFilter
    {
        [JsonProperty("pet-id")]
        public Guid PetId { get; set; }
        [JsonProperty("center-id")]
        public Guid CenterId { get; set; }
        [JsonProperty("pet-status")]
        public int PetStatus { get; set; }
        [JsonProperty("pet-type-name")]
        public string PetTypeName { get; set; }
        [JsonProperty("pet-breed-name")]
        public string PetBreedName { get; set; }
        [JsonProperty("pet-fur-color-name")]
        public string PetFurColorName { get; set; }
        //[JsonProperty("is-vaccinated")]
        //public bool IsVaccinated { get; set; }
        //[JsonProperty("is-sterilized")]
        //public bool IsSterilized { get; set; }
    }
    public class PetFieldConst
    {
        public const string INFO = "info";
        public const string DETAIL = "detail";
    }
    public class PetCreateModel
    {
        [JsonProperty("pet-status")]
        public int PetStatus { get; set; }
        [JsonProperty("center-id")]
        public Guid CenterId { get; set; }
        [JsonProperty("pet-name")]
        public string PetName { get; set; }
        [JsonProperty("pet-gender")]
        public int PetGender { get; set; }
        [JsonProperty("pet-age")]
        public int? PetAge { get; set; }
        [JsonProperty("weight")]
        public double Weight { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("pet-breed-id")]
        public Guid PetBreedId { get; set; }
        [JsonProperty("pet-fur-color-id")]
        public Guid PetFurColorId { get; set; }
        [JsonProperty("is-vaccinated")]
        public bool IsVaccinated { get; set; }
        [JsonProperty("is-sterilized")]
        public bool IsSterilized { get; set; }
    }

    public class PetDetailModel
    {
        [JsonProperty("pet-id")]
        public Guid PetId { get; set; }
        [JsonProperty("pet-name")]
        public string PetName { get; set; }
        [JsonProperty("pet-gender")]
        public int PetGender { get; set; }
        [JsonProperty("pet-age")]
        public int? PetAge { get; set; }
        [JsonProperty("weight")]
        public double Weight { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("pet-breed-id")]
        public Guid PetBreedId { get; set; }
        [JsonProperty("pet-fur-color-id")]
        public Guid PetFurColorId { get; set; }
        [JsonProperty("is-vaccinated")]
        public bool IsVaccinated { get; set; }
        [JsonProperty("is-sterilized")]
        public bool IsSterilized { get; set; }
    }
    public class PetBreedCreateModel
    {
        [JsonProperty("pet-breed-name")]
        public string PetBreedName { get; set; }
        [JsonProperty("pet-type-id")]
        public Guid PetTypeId { get; set; }
    }
    public class PetBreedUpdateModel
    {
        [JsonProperty("pet-breed-id")]
        public Guid PetBreedId { get; set; }
        [JsonProperty("pet-breed-name")]
        public string PetBreedName { get; set; }
        [JsonProperty("pet-type-id")]
        public Guid PetTypeId { get; set; }
    }
    public class PetFurColorCreateModel
    {
        [JsonProperty("pet-fur-color-name")]
        public string PetFurColorName { get; set; }
        [JsonProperty("pet-type-id")]
        public Guid PetTypeId { get; set; }
    }
    public class PetFurColorUpdateModel
    {
        [JsonProperty("pet-fur-color-id")]
        public Guid PetFurColorId { get; set; }
        [JsonProperty("pet-fur-color-name")]
        public string PetFurColorName { get; set; }
        [JsonProperty("pet-type-id")]
        public Guid PetTypeId { get; set; }
    }
}

