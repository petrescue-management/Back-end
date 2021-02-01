using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetProfile
    {
        public Guid PetId { get; set; }
        public string PetName { get; set; }
        public int PetGender { get; set; }
        public int? PetAge { get; set; }
        public double Weight { get; set; }
        public string Description { get; set; }
        public Guid PetBreedId { get; set; }
        public Guid PetFurColorId { get; set; }
        public bool IsVaccinated { get; set; }
        public bool IsSterilized { get; set; }
        public string ImageUrl { get; set; }

        public virtual PetBreed PetBreed { get; set; }
        public virtual PetFurColor PetFurColor { get; set; }
        public virtual Pet Pet { get; set; }
    }
}
