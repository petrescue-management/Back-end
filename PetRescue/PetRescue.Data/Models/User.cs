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
        }

        public Guid UserId { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public int? UserStatus { get; set; }
        public Guid? CenterId { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<NotificationToken> NotificationToken { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
