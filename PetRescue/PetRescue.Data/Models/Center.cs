using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class Center
    {
        public Center()
        {
            Pet = new HashSet<Pet>();
        }

        public Guid CenterId { get; set; }
        public string CenterName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public byte CenterStatus { get; set; }
        public Guid InsertBy { get; set; }
        public DateTime InsertAt { get; set; }
        public Guid? UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }

        public virtual ICollection<Pet> Pet { get; set; }
    }
}
