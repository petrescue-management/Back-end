using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class FinderFormDetail
    {
        public Guid RescueReportId { get; set; }
        public string ReportDescription { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string ReportLocation { get; set; }
        public string ImgReportUrl { get; set; }

        public virtual FinderForm RescueReport { get; set; }
    }
}
