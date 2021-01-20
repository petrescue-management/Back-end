using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetFurColor
    {
        public PetFurColor()
        {
            PetProfile = new HashSet<PetProfile>();
        }

        public Guid PetFurColorId { get; set; }
        public string PetFurColorName { get; set; }
        public Guid PetTypeId { get; set; }

        public virtual ICollection<PetProfile> PetProfile { get; set; }
    }
}
