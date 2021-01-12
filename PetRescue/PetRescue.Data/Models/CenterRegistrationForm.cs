using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class CenterRegistrationForm
    {
        public Guid FormId { get; set; }
        public string CenterName { get; set; }
        public string CenterAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public string CenterRegisterStatus { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
