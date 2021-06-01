using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class UserRole
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public DateTime? InsertedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsActived { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
