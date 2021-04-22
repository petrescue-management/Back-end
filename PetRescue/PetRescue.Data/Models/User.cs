using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class User
    {
        public User()
        {
            NotificationToken = new HashSet<NotificationToken>();
            UserRole = new HashSet<UserRole>();
            WorkingHistory = new HashSet<WorkingHistory>();
        }

        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public bool? IsBelongToCenter { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<NotificationToken> NotificationToken { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
        public virtual ICollection<WorkingHistory> WorkingHistory { get; set; }
    }
}
