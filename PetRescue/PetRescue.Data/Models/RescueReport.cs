using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class RescueReport
    {
        public Guid RescueReportId { get; set; }
        public int PetAttribute { get; set; }
        public int ReportStatus { get; set; }
        public string Phone { get; set; }
        public Guid? CenterId { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual RescueReportDetail RescueReportDetail { get; set; }
    }
}
