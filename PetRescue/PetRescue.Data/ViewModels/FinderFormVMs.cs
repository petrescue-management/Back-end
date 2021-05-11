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
        public DateTime? FinderDate { get; set; }
        [JsonProperty("lat")]
        public double? Lat { get; set; }
        [JsonProperty("lng")]
        public double? Lng { get; set; }
        [JsonProperty("finderFormVidUrl")]
        public string FinderFormVidUrl { get; set; }
    }
    public class FinderFormDetailModel
    {
        [JsonProperty("finderFormId")]
        public Guid FinderFormId { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("finderImageUrl")]
        public string FinderImageUrl { get; set; }
        [JsonProperty("finderName")]
        public string FinderName { get; set; }
        [JsonProperty("finderDate")]
        public DateTime? FinderDate { get; set; }
        [JsonProperty("petAttribute")]
        public int? PetAttribute { get; set;}
        [JsonProperty("finderFormStatus")]
        public int? FinderFormStatus { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("lat")]
        public double? Lat { get; set; }
        [JsonProperty("lng")]
        public double? Lng { get; set; }
        [JsonProperty("finderFormVidUrl")]
        public string FinderFormVidUrl { get; set; }
        [JsonProperty("canceledReason")]
        public string CanceledReason { get; set; }
        [JsonProperty("insertedBy")]
        public Guid? InsertedBy { get; set; }
    }
    public class FinderFormViewModel2
    {
        [JsonProperty("finderFormId")]
        public Guid FinderFormId { get; set; }
        [JsonProperty("PickerFormId")]
        public Guid PickerFormId { get; set; }
        [JsonProperty("finderDescription")]
        public string FinderDescription { get; set; }
        [JsonProperty("finderImageUrl")]
        public string FinderImageUrl { get; set; }
        [JsonProperty("finderName")]
        public string FinderName { get; set; }
        [JsonProperty("finderDate")]
        public DateTime? FinderDate { get; set; }
        [JsonProperty("petAttribute")]
        public int? PetAttribute { get; set; }
        [JsonProperty("finderFormStatus")]
        public int? FinderFormStatus { get; set; }
        [JsonProperty("pickerFormDescription")]
        public string PickerFormDescription { get; set; }
        [JsonProperty("pickerFormImg")]
        public string PickerFormImg { get; set; }
        [JsonProperty("pickerName")]
        public string PickerName { get; set; }
        [JsonProperty("pickerDate")]
        public DateTime? PickerDate { get; set; }
        [JsonProperty("finderFormVidUrl")]
        public string FinderFormVidUrl { get; set; }
        [JsonProperty("canceledReason")]
        public string CanceledReason { get; set; }
    }

}
