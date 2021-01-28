using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetRescue.Data.ViewModels
{

    public class CenterRegistrationFormModel
    {
        public Guid CenterRegistrationId { get; set; }
        public string CenterName { get; set; }
        public string CenterAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public int CenterRegistrationStatus { get; set; }
    }
    public class CreateCenterRegistrationFormModel
    {
        [Required]
        public string CenterName { get; set; }
        public string CenterAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
    }
}
