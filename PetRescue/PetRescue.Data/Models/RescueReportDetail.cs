using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class RescueReportDetail
    {
        public Guid RescueReportId { get; set; }
        public string ReportDescription { get; set; }
        public string ReportLocation { get; set; }
        public string ImgReportUrl { get; set; }

        public virtual RescueReport RescueReport { get; set; }
    }
}
