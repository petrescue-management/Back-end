using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class Pet
    {
        public Pet()
        {
            AdoptionRegistrationForm = new HashSet<AdoptionRegistrationForm>();
        }

        public Guid PetId { get; set; }
        public int PetStatus { get; set; }
        public Guid CenterId { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Center Center { get; set; }
        public virtual PetProfile PetNavigation { get; set; }
        public virtual PetDocument PetDocument { get; set; }
        public virtual ICollection<AdoptionRegistrationForm> AdoptionRegistrationForm { get; set; }
    }
}
