using System;
using System.Collections.Generic;

namespace PetRescue.Data.Models
{
    public partial class Center
    {
        public Center()
        {
            PetProfile = new HashSet<PetProfile>();
            User = new HashSet<User>();
        }

        public Guid CenterId { get; set; }
        public string CenterName { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string CenterImgUrl { get; set; }
        public int? CenterStatus { get; set; }
        public DateTime? InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual CenterRegistrationForm CenterNavigation { get; set; }
        public virtual ICollection<PetProfile> PetProfile { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
