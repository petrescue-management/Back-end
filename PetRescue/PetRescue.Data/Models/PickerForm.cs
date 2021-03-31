using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PickerForm
    {
        public Guid RescueReportId { get; set; }
        public Guid InsertBy { get; set; }
        public string PickerDescription { get; set; }
        public string PickerImageUrl { get; set; }
        public DateTime InserAt { get; set; }

        public virtual FinderForm RescueReport { get; set; }
    }
}
