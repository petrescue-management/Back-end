using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetRescue.Data.ViewModels
{
    public class NotificationToVolunteers
    {
        public Guid FinderFormId { get; set; }

        public DateTimeOffset CurrentTime { get; set; }

        public Guid InsertedBy { get; set; }

        public string path { get; set; }
    }
}
