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

        RescueReportModel UpdateRescueReportStatus(UpdateStatusModel model, Guid updateBy, Guid centerId);

        RescueReportModel CreateRescueReport(CreateRescueReportModel model, Guid insertedBy);
    }
    public partial class RescueReportRepository : BaseRepository<RescueReport, string>, IRescueReportRepository
    {
        public RescueReportRepository(DbContext context) : base(context)
        {
        }

        #region CREATE
        private RescueReport PrepareCreate(CreateRescueReportModel model, Guid insertedBy)
        {

            var report = new RescueReport
            {
                RescueReportId = Guid.NewGuid(),
                PetAttribute = model.PetAttribute,
                ReportStatus = 1,
                Phone = model.Phone,
                InsertedBy = insertedBy,
                InsertedAt = DateTime.UtcNow,
                UpdatedBy = null,
                UpdatedAt = null,
            };
            return report;
        }


        public RescueReportModel CreateRescueReport(CreateRescueReportModel model, Guid insertedBy)
        {
            var report = PrepareCreate(model, insertedBy);

            Create(report);

            var result = new RescueReportModel
            {
                RescueReportId = report.RescueReportId,
                PetAttribute = report.PetAttribute,
                ReportStatus = report.ReportStatus,
                ReportLocation = model.ReportLocation,
                ReportDescription = model.ReportDescription,
                ImgReportUrl = model.ImgReportUrl,
                Lat = model.Lat,
                Lng = model.Lng,
                
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
        private RescueReport PrepareUpdate(UpdateStatusModel model, Guid updateBy, Guid centerId)
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
                        UpdatedAt = DateTime.UtcNow,
                        CenterId = centerId,
                        Phone = r.Phone
                    }).FirstOrDefault();

            return report;
        }
       public RescueReportModel UpdateRescueReportStatus(UpdateStatusModel model, Guid updateBy, Guid centerId)
       {
            var report = PrepareUpdate(model,updateBy, centerId);

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
                        ImgReportUrl = r.RescueReportDetail.ImgReportUrl,
                        UpdatedAt = r.UpdatedAt,
                        UpdatedBy = r.UpdatedBy,
                        InsertedAt = r.InsertedAt,
                    }).FirstOrDefault();
            
            return result;
       }
        #endregion

    }
}
