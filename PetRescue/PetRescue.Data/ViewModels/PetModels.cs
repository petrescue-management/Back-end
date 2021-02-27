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
        [JsonProperty("petId")]
        public Guid PetId { get; set; }
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
        [JsonProperty("petStatus")]
        public byte PetStatus { get; set; }
        [JsonProperty("petName")]
        public string PetName { get; set; }
        [JsonProperty("petGender")]
        public byte PetGender { get; set; }
        [JsonProperty("petAge")]
        public byte? PetAge { get; set; }
        [JsonProperty("weight")]
        public double Weight { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("petBreedId")]
        public Guid PetBreedId { get; set; }
        [JsonProperty("petFurColorId")]
        public Guid PetFurColorId { get; set; }
        [JsonProperty("isVaccinated")]
        public bool IsVaccinated { get; set; }
        [JsonProperty("isSterilized")]
        public bool IsSterilized { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }

    public class PetDetailModel
    {
        [JsonProperty("petId")]
        public Guid PetId { get; set; }
        [JsonProperty("petName")]
        public string PetName { get; set; }
        [JsonProperty("petGender")]
        public byte PetGender { get; set; }
        [JsonProperty("petAge")]
        public byte? PetAge { get; set; }
        [JsonProperty("weight")]
        public double Weight { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("petBreedId")]
        public Guid PetBreedId { get; set; }
        [JsonProperty("petFurColorId")]
        public Guid PetFurColorId { get; set; }
        [JsonProperty("isVaccinated")]
        public bool IsVaccinated { get; set; }
        [JsonProperty("isSterilized")]
        public bool IsSterilized { get; set; }
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }
    }
    public class PetBreedCreateModel
    {
        [JsonProperty("petBreedName")]
        public string PetBreedName { get; set; }
        [JsonProperty("petTypeId")]
        public Guid PetTypeId { get; set; }
    }
    public class PetBreedUpdateModel
    {
        [JsonProperty("petBreedId")]
        public Guid PetBreedId { get; set; }
        [JsonProperty("petBreedName")]
        public string PetBreedName { get; set; }
        [JsonProperty("petTypeId")]
        public Guid PetTypeId { get; set; }
    }
    public class PetFurColorCreateModel
    {
        [JsonProperty("petFurColorName")]
        public string PetFurColorName { get; set; }
        [JsonProperty("petTypeId")]
        public Guid PetTypeId { get; set; }
    }
    public class PetFurColorUpdateModel
    {
        [JsonProperty("petFurColorId")]
        public Guid PetFurColorId { get; set; }
        [JsonProperty("petFurColorName")]
        public string PetFurColorName { get; set; }

    }
}

