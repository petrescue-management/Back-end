using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class Role
    {
        public Role()
        {
            UserRole = new HashSet<UserRole>();
        }

        public Guid RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
