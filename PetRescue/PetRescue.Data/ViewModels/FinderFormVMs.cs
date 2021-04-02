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
    public class FinderFormDetailModel
    {
        [JsonProperty("finderFormId")]
        public Guid FinderFormId { get; set; }
        [JsonProperty("finderDescription")]
        public string FinderDescription { get; set; }
        [JsonProperty("finderImageUrl")]
        public string FinderImageUrl { get; set; }
        [JsonProperty("finderName")]
        public string FinderName { get; set; }
        [JsonProperty("finderDate")]
        public DateTime FinderDate { get; set; }
        [JsonProperty("petAttribute")]
        public int PetAttribute { get; set;}
        [JsonProperty("finderFormStatus")]
        public int FinderFormStatus { get; set; }
        [JsonProperty("phone")]
        public string phone { get; set; }
        [JsonProperty("lat")]
        public double Lat { get; set; }
        [JsonProperty("lng")]
        public double Lng { get; set; }
    }
}
