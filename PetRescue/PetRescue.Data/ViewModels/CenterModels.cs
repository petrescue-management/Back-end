using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class CreateCenterModel
    {
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }

    public class UpdateCenterModel
    {
        public Guid CenterId { get; set; }
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int CenterStatus { get; set; }
    }

    public class CreateByApproveFormModel
    {
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
