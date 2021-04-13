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
        public string Phone { get; set; }
        public byte Gender { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? InsertedAt { get; set; }

        public virtual User User { get; set; }
    }
}
