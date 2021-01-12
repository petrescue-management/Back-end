using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class RescueReport
    {
        public Guid RescueReportId { get; set; }
        public Guid UserId { get; set; }
        public string ReportStatus { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual RescueReportDetail RescueReportDetail { get; set; }
    }
}
