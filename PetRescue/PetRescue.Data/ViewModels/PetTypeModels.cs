using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class PetTypeModel
    {
        public Guid PetTypeId { get; set; }
        public string PetTypeName { get; set; }
    }
    public class PetTypeCreateModel
    {
        [JsonProperty("pet-type-name")]
        public string PetTypeName { get; set; }
    }
    public class PetTypeUpdateModel
    {
        [JsonProperty("pet-type-id")]
        public Guid PetTypeId { get; set; }
        [JsonProperty("pet-type-name")]
        public string PetTypeName { get; set; }
    }
}
