using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class Pet
    {
        public Guid PetId { get; set; }
        public string PetName { get; set; }
        public byte? Age { get; set; }
        public string Description { get; set; }
        public string PetStatus { get; set; }
        public Guid CenterId { get; set; }
        public Guid? InsertedBy { get; set; }
        public DateTime? InsertedAt { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Post PetNavigation { get; set; }
    }
}
