using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetType
    {
        public PetType()
        {
            PetBreed = new HashSet<PetBreed>();
        }

        public Guid PetTypeId { get; set; }
        public string PetTypeName { get; set; }

        public virtual ICollection<PetBreed> PetBreed { get; set; }
    }
}
