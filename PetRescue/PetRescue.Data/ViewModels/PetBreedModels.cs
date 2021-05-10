using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class PetBreedModel
    {
        public Guid PetBreedId { get; set; }
        public string PetBreedName { get; set; }
        public Guid? PetTypeId { get; set; }
    }
    public class PetBreedDetailModel
    {
        public Guid PetBreedId { get; set; }
        public string PetBreedName { get; set; }
        public string PetTypeName { get; set; }
    }
}
