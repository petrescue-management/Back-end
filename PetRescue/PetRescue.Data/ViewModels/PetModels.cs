using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class PetModel
    {
        public Guid PetId { get; set; }
        public string PetName { get; set; }
        public int PetGender { get; set; }
        public int? PetAge { get; set; }
        public double Weight { get; set; }
        public string Description { get; set; }
        public Guid PetBreedId { get; set; }
        public string PetBreedName { get; set; }
        public Guid PetFurColorId { get; set; }
        public string PetFurColorName { get; set; }
        public bool IsVaccinated { get; set; }
        public bool IsSterilized { get; set; }
        public int PetStatus { get; set; }
        public Guid CenterId { get; set; }
    }
}
