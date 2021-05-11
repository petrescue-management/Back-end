using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PickerForm
    {
        public Guid PickerFormId { get; set; }
        public string Description { get; set; }
        public string PickerFormImgUrl { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }

        public virtual RescueDocument RescueDocument { get; set; }
    }
}
