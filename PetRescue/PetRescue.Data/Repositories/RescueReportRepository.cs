using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        SearchReturnModel SearchRescueReport(SearchModel model);

        RescueReportModel GetRescueReportById(Guid id);

        RescueReportModel UpdateRescueReport(UpdateStatusModel model);

        RescueReportModel CreateRescueReport(CreateRescueReportModel model);
    }
    public partial class RescueReportRepository : BaseRepository<RescueReport, string>, IRescueReportRepository
    {
        public RescueReportRepository(DbContext context) : base(context)
        {
        }

        #region Create
        private RescueReport PrepareCreate(CreateRescueReportModel model)
        {

            var report = new RescueReport
            {
                RescueReportId = Guid.NewGuid(),
                PetAttribute = model.PetAttribute,
                ReportStatus = 1,
                InsertedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                InsertedAt = DateTime.UtcNow,
                UpdatedBy = null,
                UpdatedAt = null
                
            };
            return report;
        }


        public RescueReportModel CreateRescueReport(CreateRescueReportModel model)
        {
            var report = PrepareCreate(model);

            Create(report);
            SaveChanges();

            var result = new RescueReportModel
            {
                RescueReportId = report.RescueReportId,
                PetAttribute = report.PetAttribute,
                ReportStatus = report.ReportStatus,
                ReportLocation = model.ReportLocation,
                ReportDescription = model.ReportDescription,
                ImgReportUrl = model.ImgReportUrl
            };

            return result;

        }
        #endregion

        #region GetById
        public RescueReportModel GetRescueReportById(Guid id)
        {
            var result = Get()
                .Where(r => r.RescueReportId.Equals(id))
                .Include(r => r.RescueReportDetail)
                .Select(r => new RescueReportModel
                {
                    RescueReportId = r.RescueReportId,
                    PetAttribute = r.PetAttribute,
                    ReportStatus = r.ReportStatus,
                    ReportDescription = r.RescueReportDetail.ReportDescription,
                    ImgReportUrl = r.RescueReportDetail.ImgReportUrl,
                    ReportLocation = r.RescueReportDetail.ReportLocation
                }).FirstOrDefault();
            return result;
        }
        #endregion

        #region Search
        public SearchReturnModel SearchRescueReport(SearchModel model)
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
        #endregion

        #region Update
       public RescueReportModel UpdateRescueReport(UpdateStatusModel model)
       {
            var report = Get()
                    .Where(r => r.RescueReportId.Equals(model.Id))
                    .Select(r => new RescueReport
                    {
                        RescueReportId = model.Id,
                        PetAttribute = r.PetAttribute,
                        ReportStatus = model.Status,
                        InsertedBy = r.InsertedBy,
                        InsertedAt = r.InsertedAt,
                        UpdatedBy = null,
                        UpdatedAt = DateTime.UtcNow
                    }).FirstOrDefault();


            Update(report);
            SaveChanges();

            var result = Get()
                    .Where(r => r.RescueReportId.Equals(model.Id))
                    .Include(r => r.RescueReportDetail)
                    .Select(r => new RescueReportModel
                    {
                        RescueReportId = model.Id,
                        PetAttribute = r.PetAttribute,
                        ReportStatus = r.ReportStatus,
                        ReportLocation = r.RescueReportDetail.ReportLocation,
                        ReportDescription = r.RescueReportDetail.ReportDescription,
                        ImgReportUrl = r.RescueReportDetail.ImgReportUrl
                    }).FirstOrDefault();

            return result;
       }
        #endregion
    }
}
