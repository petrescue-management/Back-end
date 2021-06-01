using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class CancelModel
    {
        public Guid Id { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
    }
}
