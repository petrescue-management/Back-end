using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class Adotion
    {
        public Guid AdoptionRegisterId { get; set; }
        public int AdoptionStatus { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedAt { get; set; }

        public virtual AdoptionRegisterForm AdoptionRegister { get; set; }
    }
}
