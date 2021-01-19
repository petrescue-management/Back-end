﻿using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IRescueReportRepository : IBaseRepository<RescueReport, string>
    {
        SearchReturnModel SearchRescueReport(SearchViewModel model);

        GetRescueReportByIdViewModel GetRescueReportById(Guid id);
    }
    public partial class RescueReportRepository : BaseRepository<RescueReport, string>, IRescueReportRepository
    {
        public RescueReportRepository(DbContext context) : base(context)
        {
        }

        public GetRescueReportByIdViewModel GetRescueReportById(Guid id)
        {
            var result = Get()
                .Where(r => r.RescueReportId.Equals(id))
                .Include(r => r.RescueReportDetail)
                .Select(r => new GetRescueReportByIdViewModel
                {
                    RescueReportId = r.RescueReportId,
                    PetAttribute = r.PetAttribute,
                    ReportStatus = r.ReportStatus,
                    ReportDescription = r.RescueReportDetail.ReportDescription,
                    ImgReportUrl = r.RescueReportDetail.ImgReportUrl,
                    ReportLocation = r.RescueReportDetail.ReportLocation,
                    InsertedBy = r.InsertedBy,
                    InsertedAt = r.InsertedAt
                }).FirstOrDefault();
            return result;
        }

        public SearchReturnModel SearchRescueReport(SearchViewModel model)
        {
            var records = Get().AsQueryable().Where(r => r.ReportStatus == 1);

            List<RescueReport> result = records
                .Skip((model.PageIndex - 1) * 10)
                .Take(10)
                .Select(r => new RescueReport
                {
                    RescueReportId = r.RescueReportId,
                    PetAttribute = r.PetAttribute,
                    ReportStatus = r.ReportStatus,
                    InsertedBy = r.InsertedBy,
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
    }
}
