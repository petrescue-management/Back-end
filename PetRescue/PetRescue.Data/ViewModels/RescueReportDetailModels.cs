using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class RescueReportDetailModel
    {
        [Required]
        public Guid RescueReportId { get; set; }
        public string ReportDescription { get; set; }
        public string ReportLocation { get; set; }
        public string ImgReportUrl { get; set; }

    }
}
