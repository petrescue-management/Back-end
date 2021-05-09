using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class RescueDocument
    {
        public RescueDocument()
        {
            PetProfile = new HashSet<PetProfile>();
        }

        public Guid RescueDocumentId { get; set; }
        public Guid FinderFormId { get; set; }
        public Guid PickerFormId { get; set; }
        public Guid? CenterId { get; set; }
        public int? RescueDocumentStatus { get; set; }

        public virtual FinderForm FinderForm { get; set; }
        public virtual PickerForm PickerForm { get; set; }
        public virtual ICollection<PetProfile> PetProfile { get; set; }
    }
}
