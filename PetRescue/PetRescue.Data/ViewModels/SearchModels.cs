using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class SearchModel
    {
        public string Keyword { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public int Status { get; set; }
    }

    public class SearchReturnModel
    {
        public int TotalCount { get; set; }

        public object Result { get; set; }
    }
}
