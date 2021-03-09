﻿using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class RescueReportDomain : BaseDomain
    {
        public RescueReportDomain(IUnitOfWork uow) : base(uow)
        {
        }

        #region SEARCH
        public SearchReturnModel SearchRescueReport(SearchModel model)
        {
            var records = uow.GetService<IRescueReportRepository>().Get().AsQueryable();


            if (model.Status != 0)
                records = records.Where(r => r.ReportStatus.Equals(model.Status));

            List<RescueReportModel> result = records
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(r => new RescueReportModel
                {
                    RescueReportId = r.RescueReportId,
                    PetAttribute = r.PetAttribute,
                    ReportStatus = r.ReportStatus,
                    InsertedAt = r.InsertedAt,
                    UpdatedBy = r.UpdatedBy,
                    UpdatedAt = r.UpdatedAt
                }).ToList();
            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }
        #endregion

        #region GET BY ID
        public RescueReportModel GetRescueReportById(Guid id)
        {
            var report = uow.GetService<IRescueReportRepository>().GetRescueReportById(id);
            return report;
        }
        #endregion

        #region UPDATE STATUS
        public RescueReportModel UpdateRescueReportStatus(UpdateStatusModel model, Guid updateBy)
        {
            var report = uow.GetService<IRescueReportRepository>().UpdateRescueReportStatus(model, updateBy);
            return report;
        }
        #endregion

        #region CREATE
        public RescueReportModel CreateRescueReport(CreateRescueReportModel model, Guid insertBy)
        {
            var report = uow.GetService<IRescueReportRepository>().CreateRescueReport(model, insertBy);
            uow.GetService<IRescueReportDetailRepository>().CreateRescueReportDetail(report);
            return report;           
        }
        #endregion
    }
}
