using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class PetProfile
    {
        public PetProfile()
        {
            AdoptionRegistrationForm = new HashSet<AdoptionRegistrationForm>();
            PetTracking = new HashSet<PetTracking>();
        }

        public Guid PetDocumentId { get; set; }
        public string PetName { get; set; }
        public int PetGender { get; set; }
        public int? PetAge { get; set; }
        public Guid PetBreedId { get; set; }
        public Guid PetFurColorId { get; set; }
        public string PetImgUrl { get; set; }
        public int PetStatus { get; set; }
        public string PetProfileDescription { get; set; }
        public Guid CenterId { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }

        public virtual Center Center { get; set; }
        public virtual PetBreed PetBreed { get; set; }
        public virtual PetFurColor PetFurColor { get; set; }
        public virtual PetDocument PetDocument { get; set; }
        public virtual ICollection<AdoptionRegistrationForm> AdoptionRegistrationForm { get; set; }
        public virtual ICollection<PetTracking> PetTracking { get; set; }
    }
}
