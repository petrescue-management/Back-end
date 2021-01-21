using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class RescueReportModel
    {
        public Guid RescueReportId { get; set; }
        public int PetAttribute { get; set; }
        public int ReportStatus { get; set; }
        public string ReportDescription { get; set; }
        public string ReportLocation { get; set; }
        public string ImgReportUrl { get; set; }

    }

    public class UpdateRescueReportModel
    {
        [Required]
        public Guid RescueReportId { get; set; }
        public int ReportStatus { get; set; }
        public string ReportDescription { get; set; }
        public string ReportLocation { get; set; }
        public string ImgReportUrl { get; set; }
    }

    public class CreateRescueReportModel
    {
        [Required]
        public int PetAttribute { get; set; }
        public string ReportDescription { get; set; }
        public string ReportLocation { get; set; }
        public string ImgReportUrl { get; set; }
    }
    

}
