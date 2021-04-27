using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class WorkingHistoryCreateModel
    {
        public Guid UserId { get; set; }
        public Guid CenterId { get; set; }
        public string Description { get; set; }
        public string RoleName { get; set; }

    }
    public class WorkingHistoryUpdateModel
    {
        public Guid WorkingHistoryId { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
    public class WorkingHistoryViewModel
    {
        public string CenterName { get; set; }
        public string CenterAddress { get; set; }
        public string Description { get; set; }
        public string RoleName { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime? DateEnded { get; set; }
        public bool IsActice { get; set; }
    }
}
