﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class AdoptionModel
    {
        public Guid AdoptionRegistrationId { get; set; }
        public UserModel Owner { get; set; }
        public PetProfileModel PetProfile { get; set; }
        public int AdoptionStatus { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
    public class AdoptionViewModel
    {
        public Guid AdoptionRegistrationId { get; set; }
        public UserModel Owner { get; set; }
        public int AdoptionStatus { get; set; }
        public DateTime? AdoptedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Job { get; set; }
        public string PetName { get; set; }
        public string PetImgUrl { get; set; }
        public string PetBreedName { get; set; }
        public string PetTypeName { get; set; }
        public string PetColorName { get; set; }
    }


}
