using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetDocument
    {
        public PetDocument()
        {
            PetTracking = new HashSet<PetTracking>();
        }

        public Guid PetDocumentId { get; set; }
        public Guid FinderId { get; set; }
        public string FinderDescription { get; set; }
        public int? FinderPetStatus { get; set; }
        public Guid? PickerId { get; set; }
        public string PickerDescription { get; set; }
        public Guid PetId { get; set; }

        public virtual Pet Pet { get; set; }
        public virtual ICollection<PetTracking> PetTracking { get; set; }
    }
}
