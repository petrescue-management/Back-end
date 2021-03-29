using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Extensions;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            uow.saveChanges();
            return report;
        }
        #endregion

        #region CREATE
        public async Task<List<string>> CreateRescueReportAsync(CreateRescueReportModel model, Guid insertedBy,string path)
        {
            var report = uow.GetService<IRescueReportRepository>().CreateRescueReport(model, insertedBy);
            uow.GetService<IRescueReportDetailRepository>().CreateRescueReportDetail(report);
            var centerDomain = uow.GetService<CenterDomain>();
            var listCenters = centerDomain.GetListCenterLocation();
            var googleMapExtension = new GoogleMapExtensions();

            //var orgirin = model.Lat + ", " + model.Lng;
            //var result = googleMapExtension.FindListShortestCenter(orgirin, listCenters);
            //var listTopic = new List<string>();
            //if(result.Count !=0)
            //{
            //    if(result.Count >= 2)
            //    {
            //        listTopic.Add(result[0].CenterId);
            //        listTopic.Add(result[1].CenterId);
            //    }
            //    else
            //    {
            //        listTopic.Add(result[0].CenterId);
            //    }
            //}                    
            //await uow.GetService<NotificationTokenDomain>().NotificationForListVolunteerOfCenter(path, listTopic);

            uow.saveChanges();
            //return report.PetAttribute;
            var rescueRepo = uow.GetService<IRescueReportRepository>();
            var list = new List<string>();
            int result = -1;
            var check = false;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
            Thread newThread = new Thread(
                delegate (object rescueId)
                {
                    Thread newThread2 = new Thread(delegate() {
                        while (true)
                        {
                            result = GetRescueReportById(Guid.Parse(rescueId.ToString())).ReportStatus;
                            list.Add(result.ToString());
                            if(result != 1)
                            {
                                check = true;
                                break;
                            }
                            Thread.Sleep(TimeSpan.FromSeconds(5));
                        }
                    });
                    newThread2.Start();
                    
                }
            );
            newThread.Start(report.RescueReportId);
            for(int i = 0; i< 120; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                if (check) break;
            }
            return list;
        }

        #endregion
    }
}
