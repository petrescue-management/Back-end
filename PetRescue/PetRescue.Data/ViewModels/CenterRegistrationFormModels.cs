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
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Description { get; set; }
        public int CenterRegistrationStatus { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ImageUrl { get; set; }
    }
    public class CreateCenterRegistrationFormModel
    {
        [Required]
        public string CenterName { get; set; }
        public string CenterAddress { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; } 
    }
    public class CenterRegistrationFormUpdateModel
    {
        public Guid CenterRegistrationFormId { get; set; }
        public int Status { get; set; }
        public bool IsEmail { get; set; }
        public bool IsPhone { get; set; }
        public bool IsAddress { get; set; }
        public bool IsName { get; set; }
        public string AnotherReason { get; set; }
    }
    public class CenterRegistrationFormViewModel
    {
        public string Email { get; set; }
        public string CenterName { get; set; }
    }
}
