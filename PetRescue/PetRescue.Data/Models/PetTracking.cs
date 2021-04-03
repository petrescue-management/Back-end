using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetTracking
    {
        public Guid PetTrackingId { get; set; }
        public Guid PetProfileId { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public bool? IsVaccinated { get; set; }
        public bool? IsSterilized { get; set; }
        public string PetTrackingImgUrl { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }

        public virtual PetProfile PetProfile { get; set; }
    }
}
