using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class CenterRegistrationForm
    {
        public Guid CenterRegistrationId { get; set; }
        public string CenterName { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string CenterAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public int CenterRegistrationStatus { get; set; }
        public string ImageUrl { get; set; }
        public string RejectedReason { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Center Center { get; set; }
    }
}
