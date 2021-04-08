using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetDocument
    {
        public Guid PetDocumentId { get; set; }
        public Guid FinderFormId { get; set; }
        public Guid PickerFormId { get; set; }
        public Guid? CenterId { get; set; }

        public virtual FinderForm FinderForm { get; set; }
        public virtual PickerForm PickerForm { get; set; }
    }
}
