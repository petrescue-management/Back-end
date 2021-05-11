using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetBreed
    {
        public PetBreed()
        {
            PetProfile = new HashSet<PetProfile>();
        }

        public Guid PetBreedId { get; set; }
        public string PetBreedName { get; set; }
        public Guid? PetTypeId { get; set; }

        public virtual PetType PetType { get; set; }
        public virtual ICollection<PetProfile> PetProfile { get; set; }
    }
}
