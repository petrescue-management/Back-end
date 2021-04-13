using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class PetTrackingCreateModel
    {
        public Guid PetProfileId { get; set; }
        public bool isVaccinated { get; set; }
        public bool isSterilized { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }

    }
    public class PetTrackingViewModel
    {
        public Guid PetTrackingId { get; set; }
        public bool? IsVaccinated { get; set; }
        public bool? IsSterilized { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public DateTime InsertAt { get; set; }
        public string Author { get; set; }
    }


    public class CreatePetTrackingByUserModel
    {
        public Guid PetProfileId { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
}
