using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IRescueReportDetailRepository : IBaseRepository<RescueReportDetail, string>
    {
        RescueReportDetailModel CreateRescueReportDetail(RescueReportModel model);
        UpdateRescueReportModel GetRescueReportDetailWithStatus(UpdateStatusModel model);
    }

    public partial class RescueReportDetailRepository : BaseRepository<RescueReportDetail, string>, IRescueReportDetailRepository
    {
        public RescueReportDetailRepository(DbContext context) : base(context)
        {
        }

        #region Create
        private RescueReportDetail PrepareCreate(RescueReportModel model)
        {
            var report = new RescueReportDetail
            {
                RescueReportId = model.RescueReportId,
                ReportLocation = model.ReportLocation,
                ReportDescription = model.ReportDescription,
                ImgReportUrl = model.ImgReportUrl
            };
            return report;
        }

        public RescueReportDetailModel CreateRescueReportDetail(RescueReportModel model)
        {
            var report = PrepareCreate(model);

            Create(report);
            SaveChanges();

            var result = new RescueReportDetailModel
            {
                RescueReportId = report.RescueReportId,
                ReportLocation = report.ReportLocation,
                ReportDescription = report.ReportDescription,
                ImgReportUrl = report.ImgReportUrl
            };
            return result;
        }
        #endregion

        #region GetWithStatus
        public UpdateRescueReportModel GetRescueReportDetailWithStatus(UpdateStatusModel model)
        {
            var report = Get()
                .Where(r => r.RescueReportId.Equals(model.Id))
                .Select(r => new UpdateRescueReportModel
                {
                    RescueReportId = model.Id,
                    ReportStatus = model.Status,
                    ReportLocation = r.ReportLocation,
                    ReportDescription = r.ReportDescription,
                    ImgReportUrl = r.ImgReportUrl
                }).FirstOrDefault();

            return report;
        }
        #endregion
    }
}
