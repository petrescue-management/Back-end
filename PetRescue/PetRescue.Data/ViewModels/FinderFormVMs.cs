using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class FinderFormViewModel
    {
        [JsonProperty("finderDescription")]
        public string FinderDescription { get; set; }
        [JsonProperty("finderImageUrl")]
        public string FinderImageUrl { get; set; }
        [JsonProperty("finderName")]
        public string FinderName { get; set; }
        [JsonProperty("finderDate")]
        public DateTime FinderDate { get; set; }
    }
}
