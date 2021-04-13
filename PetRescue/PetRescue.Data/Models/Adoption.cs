using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class Adoption
    {
        public Guid AdoptionRegistrationId { get; set; }
        public int AdoptionStatus { get; set; }
        public string ReturnedReason { get; set; }
        public DateTime InsertedAt { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }

        public virtual AdoptionRegistrationForm AdoptionRegistration { get; set; }
    }
}
