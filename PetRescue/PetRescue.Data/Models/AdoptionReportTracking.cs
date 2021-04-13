using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class AdoptionReportTracking
    {
        public Guid AdoptionReportTrackingId { get; set; }
        public Guid PetProfileId { get; set; }
        public string Description { get; set; }
        public string AdoptionReportTrackingImgUrl { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }

        public virtual PetProfile PetProfile { get; set; }
    }
}
