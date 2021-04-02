using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class VolunteerRegistrationFormCreateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime Dob { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public Guid CenterId { get; set; }
    }
    public class VolunteerRegistrationFormUpdateModel
    {
        public Guid VolunteerRegistrationFormId { get; set; }
        public int Status { get; set; }
        public bool IsEmail { get; set; }
        public bool IsPhone { get; set; }
        public bool IsName { get; set; }
        public string AnotherReason { get; set; }
    }
    public class VolunteerViewModel
    {
        public int Count { get; set; }
        public List<VolunteerRegistrationFormViewModel> List { get; set; }
    }
    public class VolunteerRegistrationFormViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public DateTime Dob { get; set; }
        public int Gender { get; set; }
        public string Email { get; set; }
        public Guid CenterId { get; set; }
        public int Status { get; set; }
        public Guid FormId { get; set; }
        public DateTime InsertAt { get; set; }
    }
}
