using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class PickerFormModel
    {
        public Guid PickerFormId { get; set; }
        public string Description { get; set; }
        public string PickerImageUrl { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }
    }

    public class CreatePickerFormModel
    {
        public string Description { get; set; }
        public string PickerImageUrl { get; set; }
    }
}
