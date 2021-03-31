using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class VolunteerRegistrationForm
    {
        public Guid VolunteerRegistrationFormId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public DateTime Dob { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public Guid CenterId { get; set; }
    }
}
