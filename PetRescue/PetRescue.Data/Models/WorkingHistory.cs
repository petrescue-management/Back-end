using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class WorkingHistory
    {
        public Guid WorkingHistoryId { get; set; }
        public Guid UserId { get; set; }
        public Guid CenterId { get; set; }
        public string Description { get; set; }
        public string RoleName { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        public bool IsActive { get; set; }

        public virtual Center Center { get; set; }
        public virtual User User { get; set; }
    }
}
