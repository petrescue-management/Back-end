using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class RescueReport
    {
        public Guid RescueReportId { get; set; }
        public byte PetAttribute { get; set; }
        public byte ReportStatus { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual RescueReportDetail RescueReportDetail { get; set; }
    }
}
