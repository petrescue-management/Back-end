using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class RescueReportDomain : BaseDomain
    {
        public RescueReportDomain(IUnitOfWork uow) : base(uow)
        {
        }

        public SearchReturnModel SearchRescueReport(SearchViewModel model)
        {
            var forms = uow.GetService<IRescueReportRepository>().SearchRescueReport(model);
            return forms;
        }
    }
}
