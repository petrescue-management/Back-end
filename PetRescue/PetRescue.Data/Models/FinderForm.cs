using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class FinderForm
    {
        public Guid FinderFormId { get; set; }
        public string FinderDescription { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string FinderFormImgUrl { get; set; }
        public int PetAttribute { get; set; }
        public int FinderFormStatus { get; set; }
        public string Phone { get; set; }
        public string CancelledReason { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatetedAt { get; set; }

        public virtual PetDocument PetDocument { get; set; }
    }
}
