using PetRescue.Data.Models;
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

        public SearchReturnModel SearchRescueReport(SearchModel model)
        {
            var reports = uow.GetService<IRescueReportRepository>().SearchRescueReport(model);
            return reports;
        }

        public RescueReportModel GetRescueReportById(Guid id)
        {
            var report = uow.GetService<IRescueReportRepository>().GetRescueReportById(id);
            return report;
        }

        public RescueReportModel UpdateRescueReport(UpdateStatusModel model)
        {
            var update_model = uow.GetService<IRescueReportDetailRepository>().GetRescueReportDetailWithStatus(model);
            var report = uow.GetService<IRescueReportRepository>().UpdateRescueReport(update_model);
            return report;
        }

        public RescueReportModel CreateRescueReport(CreateRescueReportModel model)
        {
            var report = uow.GetService<IRescueReportRepository>().CreateRescueReport(model);
            uow.GetService<IRescueReportDetailRepository>().CreateRescueReportDetail(report);
            return report;           
        }
    }
}
