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
        public Guid UserId { get; set; }
        public Guid CenterId { get; set; }
        public string Description { get; set; }
        public string RoleName { get; set; }

        public bool IsActice { get; set; }
    }
}
