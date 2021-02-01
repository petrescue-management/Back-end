using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class AdoptionModel
    {
        public Guid AdoptionRegisterId { get; set; }
        public Guid OwnerId { get; set; }
        public string OwnerName { get; set; }
        public Guid PetId { get; set; }

        //public string PetName { get; set; }
        public int AdoptionStatus { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }

}
