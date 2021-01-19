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
            var reports = uow.GetService<IRescueReportRepository>().SearchRescueReport(model);
            return reports;
        }

        public GetRescueReportByIdViewModel GetRescueReportById(Guid id)
        {
            var report = uow.GetService<IRescueReportRepository>().GetRescueReportById(id);
            return report;
        }

        public void UpdateRescueReport(UpdateRescueReportModel model)
        {
            uow.GetService<IRescueReportRepository>().UpdateRescueReport(model);
        }
    }
}
