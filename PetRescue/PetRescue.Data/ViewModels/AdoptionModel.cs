using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class AdoptionModel
    {
        public Guid AdoptionRegistrationId { get; set; }
        public UserModel Owner { get; set; }
        public PetModel Pet { get; set; }
        public int AdoptionStatus { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
     
}
