using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class User
    {
        public User()
        {
            UserRole = new HashSet<UserRole>();
        }

        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public Guid? CenterId { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
