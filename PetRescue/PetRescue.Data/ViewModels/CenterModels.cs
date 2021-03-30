using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class CenterModel
    {
        public Guid CenterId { get; set; }
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double? Lat { get; set; }
        public double? Long { get; set; }
        public int CenterStatus { get; set; }
        public DateTime InsertAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public string ImageUrl { get; set; }
    }


    public class CreateCenterModel
    {
        [Required]
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
    }


    public class UpdateCenterModel
    {
        [Required]
        public Guid CenterId { get; set; }
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public byte CenterStatus { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class CenterLocationModel
    {
        public string CenterId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class CenterStatistic
    {
        public int RescueCase { get; set; }
        public int PetAdoption { get; set; }
        public int PetFindTheOwner { get; set; }
    }
}
