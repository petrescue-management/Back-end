using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetDocument
    {
        public PetDocument()
        {
            PetProfile = new HashSet<PetProfile>();
        }

        public Guid PetDocumentId { get; set; }
        public Guid FinderFormId { get; set; }
        public Guid PickerFormId { get; set; }
        public string PetDocumentDescription { get; set; }
        public int PetAttribute { get; set; }

        public virtual FinderForm FinderForm { get; set; }
        public virtual PickerForm PickerForm { get; set; }
        public virtual ICollection<PetProfile> PetProfile { get; set; }
    }
}
