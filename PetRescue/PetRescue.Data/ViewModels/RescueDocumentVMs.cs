using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static PetRescue.Data.ViewModels.PetProfileModel;

namespace PetRescue.Data.ViewModels
{
    public class RescueDocumentViewModel
    {
        public PickerFormViewModel PickerForm { get; set; } 
        public FinderFormViewModel FinderForm { get; set; }
        public int PetAttribute { get; set; }
        public PetProfileModel PetProfile { get; set; }
        public List<PetTrackingViewModel> ListTracking { get; set; }
    }
    public class RescueDocumentModel
    {
        public PickerFormViewModel PickerForm { get; set; }
        public FinderFormViewModel FinderForm { get; set; }
        public Guid PetDocumentId { get; set; }
        public int? PetDocumentStatus { get; set; }
    }
    public class RescueDocumentUpdateModel
    {
        [JsonProperty("petDocumentId")]
        public Guid PetDocumentId { get; set; }
        [JsonProperty("petDocumentStatus")]
        public int PetDocumentStatus { get; set; }
        [JsonProperty("pets")]
        public List<CreatePetProfileModel> Pets {get;set ;}
    }
    public class RescueDocumentCreateModel
    {
        public Guid PickerFormId { get; set; }
        public Guid FinderFormId { get; set; }
    }

}
