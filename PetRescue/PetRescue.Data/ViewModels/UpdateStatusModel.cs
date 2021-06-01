using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class UpdateStatusModel
    {
        [Required]
        public Guid Id { get; set; }
        public byte Status { get; set; }
    }
    public class UpdateViewModel
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
    }
    public class CancelViewModel
    {
        public Guid Id { get; set; }
        public string Reason { get; set; }
    }
    public class UpdateRegistrationCenter
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
        public bool IsMail { get; set; }
        public bool IsAddress { get; set; }
        public bool IsPhone { get; set; }
        public bool IsImage { get; set; }
        public string AnotherReason { get; set; }
    }
    public class UpdateStatusFinderFormModel
    {
        [Required]
        public Guid Id { get; set; }
        public byte Status { get; set; }
        public Guid? CenterId { get; set; }
    }
    public class UpdateStatusFinderFormModel2
    {
        [Required]
        public Guid Id { get; set; }
        public byte Status { get; set; }
    }
}
