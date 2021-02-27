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

        RescueReportModel GetRescueReportById(Guid id);

        RescueReportModel UpdateRescueReportStatus(UpdateStatusModel model, Guid updateBy);

        RescueReportModel CreateRescueReport(CreateRescueReportModel model, Guid insertBy);
    }
    public partial class RescueReportRepository : BaseRepository<RescueReport, string>, IRescueReportRepository
    {
        public RescueReportRepository(DbContext context) : base(context)
        {
        }

        #region CREATE
        private RescueReport PrepareCreate(CreateRescueReportModel model, Guid insertBy)
        {

            var report = new RescueReport
            {
                RescueReportId = Guid.NewGuid(),
                PetAttribute = model.PetAttribute,
                ReportStatus = 1,
                InsertedBy = insertBy,
                InsertedAt = DateTime.UtcNow,
                UpdatedBy = null,
                UpdatedAt = null
                
            };
            return report;
        }


        public RescueReportModel CreateRescueReport(CreateRescueReportModel model, Guid insertBy)
        {
            var report = PrepareCreate(model, insertBy);

            Create(report);

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

        #region GET BY ID
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

        #region UPDATE STATUS
        private RescueReport PrepareUpdate(UpdateStatusModel model, Guid updateBy)
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
                        UpdatedBy = updateBy,
                        UpdatedAt = DateTime.UtcNow
                    }).FirstOrDefault();

            return report;
        }
       public RescueReportModel UpdateRescueReportStatus(UpdateStatusModel model, Guid updateBy)
       {
            var report = PrepareUpdate(model,updateBy);

            Update(report);

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
