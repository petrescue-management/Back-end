using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class NotificationToken
    {
        public Guid NotificationTokenId { get; set; }
        public Guid? UserId { get; set; }
        public string DeviceToken { get; set; }
        public string ApplicationName { get; set; }
        public bool? IsActived { get; set; }

        public virtual User User { get; set; }
    }
}
