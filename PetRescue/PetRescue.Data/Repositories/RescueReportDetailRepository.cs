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
    }

    public partial class RescueReportDetailRepository : BaseRepository<RescueReportDetail, string>, IRescueReportDetailRepository
    {
        public RescueReportDetailRepository(DbContext context) : base(context)
        {
        }

        #region CREATE
        private RescueReportDetail PrepareCreate(RescueReportModel model)
        {
            var report = new RescueReportDetail
            {
                RescueReportId = model.RescueReportId,
                ReportLocation = model.ReportLocation,
                ReportDescription = model.ReportDescription,
                ImgReportUrl = model.ImgReportUrl,
                Lat = model.Lat,
                Lng = model.Lng
            };
            return report;
        }

        public RescueReportDetailModel CreateRescueReportDetail(RescueReportModel model)
        {
            var report = PrepareCreate(model);

            Create(report);

            var result = new RescueReportDetailModel
            {
                RescueReportId = report.RescueReportId,
                ReportLocation = report.ReportLocation,
                ReportDescription = report.ReportDescription,
                ImgReportUrl = report.ImgReportUrl,
                Lat = model.Lat,
                Lng = model.Lng
            };
            return result;
        }
        #endregion
    }
}
