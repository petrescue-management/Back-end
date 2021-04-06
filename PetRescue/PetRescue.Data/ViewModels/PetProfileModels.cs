using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class PetProfileModel
    {
        public Guid PetProfileId { get; set; }
        public Guid PetDocumentId { get; set; }
        public string PetName { get; set; }
        public int PetGender { get; set; }
        public int? PetAge { get; set; }
        public Guid PetBreedId { get; set; }
        public string PetBreedName { get; set; }
        public Guid PetFurColorId { get; set; }
        public string PetFurColorName { get; set; }
        public string PetImgUrl { get; set; }
        public int PetStatus { get; set; }
        public string PetProfileDescription { get; set; }
        public Guid CenterId { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public CenterProfileViewModel CenterProfile { get; set; }

        public class CreatePetProfileModel
        {
            public Guid PetDocumentId { get; set; }
            public string PetName { get; set; }
            public int PetGender { get; set; }
            public int? PetAge { get; set; }
            public Guid PetBreedId { get; set; }
            public Guid PetFurColorId { get; set; }
            public string PetImgUrl { get; set; }
            public int PetStatus { get; set; }
            public string PetProfileDescription { get; set; }
        }

        public class UpdatePetProfileModel
        {
            public Guid PetDocumentId { get; set; }
            public string PetName { get; set; }
            public int PetGender { get; set; }
            public int? PetAge { get; set; }
            public Guid PetBreedId { get; set; }
            public Guid PetFurColorId { get; set; }
            public string PetImgUrl { get; set; }
            public int PetStatus { get; set; }
            public string PetProfileDescription { get; set; }
        }

        public class GetPetByTypeNameModel
        {
            public string TypeName { get; set; }
            public List<PetProfileModel> result { get; set; }
        }

        public class SearchPetProfileModel
        {
            public Guid PetTypeId { get; set; }
            public int PetGender { get; set; }
            public int? PetAge { get; set; }
            public Guid PetBreedId { get; set; }
            public Guid PetFurColorId { get; set; }
            public int PageIndex { get; set; } = 1;

            public int PageSize { get; set; } = 10;

            public int PetStatus { get; set; }


        }
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
    }
}
