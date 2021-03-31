using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class PetProfileModel
    {
        public Guid PetId { get; set; }
        public string PetName { get; set; }
        public int PetGender { get; set; }
        public int? PetAge { get; set; }
        public string Description { get; set; }
        public Guid PetBreedId { get; set; }
        public Guid PetFurColorId { get; set; }

    }
}
