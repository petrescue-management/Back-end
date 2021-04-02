using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class UserRole
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? InsertedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
