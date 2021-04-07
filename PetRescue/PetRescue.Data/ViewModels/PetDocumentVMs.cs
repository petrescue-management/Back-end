using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
