using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class UserRole
    {
        public Guid RoleId { get; set; }
        public Guid UserId { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public Guid? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
