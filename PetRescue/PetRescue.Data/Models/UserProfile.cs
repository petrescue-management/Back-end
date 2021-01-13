using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class UserProfile
    {
        public Guid UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool Gender { get; set; }
    }
}
