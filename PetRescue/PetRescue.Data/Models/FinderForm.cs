using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class FinderForm
    {
        public Guid FinderFormId { get; set; }
        public string Description { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string FinderFormImgUrl { get; set; }
        public string FinderFormVidUrl { get; set; }
        public int? PetAttribute { get; set; }
        public int? FinderFormStatus { get; set; }
        public string Phone { get; set; }
        public string DroppedReason { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual RescueDocument RescueDocument { get; set; }
    }
}
