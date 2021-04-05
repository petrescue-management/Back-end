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
}
