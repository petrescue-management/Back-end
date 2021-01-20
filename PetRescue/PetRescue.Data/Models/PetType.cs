using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetType
    {
        public PetType()
        {
            PetProfile = new HashSet<PetProfile>();
        }

        public Guid PetTypeId { get; set; }
        public string PetTypeName { get; set; }

        public virtual ICollection<PetProfile> PetProfile { get; set; }
    }
}
