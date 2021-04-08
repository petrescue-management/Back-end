using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static PetRescue.Data.ViewModels.PetProfileModel;

namespace PetRescue.Data.ViewModels
{
    public class PetDocumentViewModel
    {
        public PickerFormViewModel PickerForm { get; set; } 
        public FinderFormViewModel FinderForm { get; set; }
        public int PetAttribute { get; set; }
        public PetProfileModel PetProfile { get; set; }
        public List<PetTrackingViewModel> ListTracking { get; set; }
    }
    public class PetDocumentModel
    {
        public PickerFormViewModel PickerForm { get; set; }
        public FinderFormViewModel FinderForm { get; set; }
        public Guid PetDocumentId { get; set; }
        public int? PetDocumentStatus { get; set; }
    }
    public class PetDocumentUpdateModel
    {
        [JsonProperty("petDocumentId")]
        public Guid PetDocumentId { get; set; }
        [JsonProperty("petDocumentStatus")]
        public int PetDocumentStatus { get; set; }
        [JsonProperty("pets")]
        public List<CreatePetProfileModel> Pets {get;set ;}
    }

}
