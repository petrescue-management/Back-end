using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class PetTrackingCreateModel
    {
        public Guid PetDocumentId { get; set; }
        public bool isVaccinated { get; set; }
        public bool isSterilized { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }

    }
    public class PetTrackingViewModel
    {
        public bool isVaccinated { get; set; }
        public bool isSterilized { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public double Weight { get; set; }
        public DateTime InsertAt { get; set; }
    }
}
