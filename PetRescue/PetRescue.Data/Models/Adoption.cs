using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class Adoption
    {
        public Guid AdoptionRegisterId { get; set; }
        public byte AdoptionStatus { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual AdoptionRegisterForm AdoptionRegister { get; set; }
    }
}
