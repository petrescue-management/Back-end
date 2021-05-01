using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetProfile
    {
        public PetProfile()
        {
            AdoptionRegistrationForm = new HashSet<AdoptionRegistrationForm>();
            AdoptionReportTracking = new HashSet<AdoptionReportTracking>();
            PetTracking = new HashSet<PetTracking>();
        }

        public Guid PetProfileId { get; set; }
        public Guid? RescueDocumentId { get; set; }
        public string PetName { get; set; }
        public int PetGender { get; set; }
        public int PetAge { get; set; }
        public Guid PetBreedId { get; set; }
        public Guid PetFurColorId { get; set; }
        public string PetImgUrl { get; set; }
        public string PetProfileDescription { get; set; }
        public int PetStatus { get; set; }
        public Guid CenterId { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Center Center { get; set; }
        public virtual PetBreed PetBreed { get; set; }
        public virtual PetFurColor PetFurColor { get; set; }
        public virtual RescueDocument RescueDocument { get; set; }
        public virtual ICollection<AdoptionRegistrationForm> AdoptionRegistrationForm { get; set; }
        public virtual ICollection<AdoptionReportTracking> AdoptionReportTracking { get; set; }
        public virtual ICollection<PetTracking> PetTracking { get; set; }
    }
}
