using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class AdoptionReportTrackingCreateModel
    {
        public Guid PetProfileId { get; set; }
        public string Description { get; set; }
        public string AdoptionReportImage { get; set; }
    }
    public class AdoptionReportTrackingUpdateModel
    {
        public Guid AdoptionReportTrackingId { get; set; }
        public string Description { get; set; }
        public string AdoptionReportImage { get; set; }
    }
    public class AdoptionReportTrackingViewModel
    {
        public Guid AdoptionReportTrackingId { get; set; }
        public Guid PetProfileId { get; set; }
        public string Description { get; set; }
        public string AdoptionReportTrackingImgUrl { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public string Author { get; set; }
    }
    public class AdoptionReportTrackingViewMobileModel
    {
        public Guid AdoptionReportTrackingId { get; set; }
        public Guid PetProfileId { get; set; }
        public string Description { get; set; }
        public string AdoptionReportTrackingImgUrl { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
    }
}