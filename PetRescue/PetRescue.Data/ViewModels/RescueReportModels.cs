using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class GetRescueReportByIdViewModel
    {
        public Guid RescueReportId { get; set; }
        public int PetAttribute { get; set; }
        public int ReportStatus { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public string ReportDescription { get; set; }
        public string ReportLocation { get; set; }
        public string ImgReportUrl { get; set; }

    }

    public class UpdateRescueReportModel
    {
        public Guid RescueReportId { get; set; }
        public int ReportStatus { get; set; }
    }

    public class CreateRescueReportModel
    {
        public int PetAttribute { get; set; }
        public string ReportDescription { get; set; }
        public string ReportLocation { get; set; }
        public string ImgReportUrl { get; set; }
    }
}
