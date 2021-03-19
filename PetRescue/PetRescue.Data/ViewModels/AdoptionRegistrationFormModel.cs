using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class AdoptionRegistrationFormModel
    {
        public Guid AdoptionRegistrationId { get; set; }
        public PetModel Pet { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Job { get; set; }
        public string Address { get; set; }
        public int HouseType { get; set; }
        public int FrequencyAtHome { get; set; }
        public bool HaveChildren { get; set; }
        public int? ChildAge { get; set; }
        public bool BeViolentTendencies { get; set; }
        public bool HaveAgreement { get; set; }
        public int HavePet { get; set; }
        public int AdoptionRegistrationStatus { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdateAt { get; set; }

    }

    public class CreateAdoptionRegistrationFormModel
    {
        public Guid PetId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Job { get; set; }
        public string Address { get; set; }
        public int HouseType { get; set; }
        public int FrequencyAtHome { get; set; }
        public bool HaveChildren { get; set; }
        public int? ChildAge { get; set; }
        public bool BeViolentTendencies { get; set; }
        public bool HaveAgreement { get; set; }
        public int HavePet { get; set; }
        public int AdoptionRegistrationStatus { get; set; }

    }
    public class AdoptionRegistrationFormViewModel
    {
        public Guid AdoptionRegistrationId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Job { get; set; }
        public string Address { get; set; }
        public int HouseType { get; set; }
        public int FrequencyAtHome { get; set; }
        public bool HaveChildren { get; set; }
        public int? ChildAge { get; set; }
        public bool BeViolentTendencies { get; set; }
        public bool HaveAgreement { get; set; }
        public int HavePet { get; set; }
        public int AdoptionRegistrationStatus { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
    public class ListRegisterAdoptionOfPetViewModel
    {
        public PetModel Pet { get; set; }
        public List<AdoptionRegistrationFormViewModel> AdoptionRegisterforms { get; set; }
    }
}
