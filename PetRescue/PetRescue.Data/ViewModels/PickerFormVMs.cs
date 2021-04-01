using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class PickerFormCreateModel
    {
        public string PickerDescription { get; set; }
        public string PickerImageUrl { get; set; }
        public Guid RescueReportId { get; set; }
    }
    public class PickerFormViewModel
    {
        [JsonProperty("pickerDescription")]
        public string PickerDescription { get; set; }
        [JsonProperty("pickerImageUrl")]
        public string PickerImageUrl { get; set; }
        [JsonProperty("pickerName")]
        public string PickerName { get; set; }
        [JsonProperty("pickerDate")]
        public DateTime PickerDate { get; set; }
    }
}
