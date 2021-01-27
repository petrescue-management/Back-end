using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.ViewModels
{
    public class SearchModel
    {
        public string Keyword { get; set; }

        public int PageIndex { get; set; } = 1;
    }

    public class SearchReturnModel
    {
        public int TotalCount { get; set; }

        public object Result { get; set; }
    }
}
