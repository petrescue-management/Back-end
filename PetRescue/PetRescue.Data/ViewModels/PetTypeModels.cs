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
        [JsonProperty("petTypeName")]
        public string PetTypeName { get; set; }
    }
    public class PetTypeUpdateModel
    {
        [JsonProperty("petTypeId")]
        public Guid PetTypeId { get; set; }
        [JsonProperty("petTypeName")]
        public string PetTypeName { get; set; }
    }
}
