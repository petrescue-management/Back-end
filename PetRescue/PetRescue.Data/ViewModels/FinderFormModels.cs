using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class FinderFormModel
    {
        public Guid FinderFormId { get; set; }
        public string FinderDescription { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string FinderFormImgUrl { get; set; }
        public int PetAttribute { get; set; }
        public int FinderFormStatus { get; set; }
        public string Phone { get; set; }
        public Guid InsertedBy { get; set; }
        public DateTime InsertedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string FinderFormVideoUrl { get; set; }
        public string CanceledReason { get; set; }
    }

    public class CreateFinderFormModel
    {
        public string FinderDescription { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string FinderFormImgUrl { get; set; }
        public int PetAttribute { get; set; }
        public string Phone { get; set; }
        public string FinderFormVideoUrl { get; set; }
    }
}
