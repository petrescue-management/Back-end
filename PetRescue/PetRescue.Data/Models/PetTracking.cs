using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetTracking
    {
        public Guid PetTrackingId { get; set; }
        public Guid DocumentRecordId { get; set; }
        public int? Weight { get; set; }
        public bool? IsVaccinated { get; set; }
        public bool? IsSterilized { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public Guid InsertBy { get; set; }
        public DateTime InsertAt { get; set; }

        public virtual PetDocument DocumentRecord { get; set; }
    }
}
