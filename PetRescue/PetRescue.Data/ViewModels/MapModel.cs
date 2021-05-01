using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class MapModel
    {
        public string[] destination_addresses { get; set; }
        public string[] origin_addresses { get; set; }
        public List<Rows> rows { get; set; }
        public string status { get; set; }

    }
    public class Rows
    {
        public List<Elements> elements { get; set; }
    }
    public class Elements
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }
    public class Distance
    {
        public string text { get; set; }
        public double value { get; set; }
    }
    public class Duration
    {
        public string text { get; set; }
        public double value { get; set; }
    }

    public class CenterDistanceModel : IComparable<CenterDistanceModel>
    {
        public double Value { get; set; }
        public string CenterId { get; set; }
        public int CompareTo(CenterDistanceModel model)
        {
            if (model == null) return 1;
            else
            {
                return this.Value.CompareTo(model.Value);
            }
        }
    }
}
