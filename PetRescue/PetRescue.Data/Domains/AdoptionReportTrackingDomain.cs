using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class AdoptionReportTrackingDomain : BaseDomain
    {
        public AdoptionReportTrackingDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public bool Create(AdoptionReportTrackingCreateModel model, Guid insertedBy, string path)
        {
            var adoptionReport = uow.GetService<IAdoptionReportTrackingRepository>().Create(model, insertedBy);
            if (adoptionReport != null)
            {
                uow.saveChanges();
                return true;
            }
            return false;
        }
        public bool Edit(AdoptionReportTrackingUpdateModel model, Guid insertedBy)
        {
            var adoptionReport = uow.GetService<IAdoptionReportTrackingRepository>().Edit(model, insertedBy);
            if (adoptionReport != null)
            {
                uow.saveChanges();
                return true;
            }
            return false;
        }
        public AdoptionReportTrackingViewModel GetByAdoptionReportTrackingId(Guid reportId)
        {
            var adoptionReport = uow.GetService<IAdoptionReportTrackingRepository>().Get().FirstOrDefault(s => s.AdoptionReportTrackingId.Equals(reportId));
            var userRepo = uow.GetService<IUserRepository>();
            var result = new AdoptionReportTrackingViewModel();
            if(adoptionReport != null)
            {
                var userCreated = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(adoptionReport.InsertedBy));
                result.AdoptionReportTrackingId = adoptionReport.AdoptionReportTrackingId;
                result.InsertedAt = adoptionReport.InsertedAt;
                result.InsertedBy = adoptionReport.InsertedBy;
                result.Description = adoptionReport.Description;
                result.AdoptionReportTrackingImgUrl = adoptionReport.AdoptionReportTrackingImgUrl;
                result.PetProfileId = adoptionReport.PetProfileId;
                result.Author = userCreated.UserProfile.LastName +" " + userCreated.UserProfile.FirstName;
            }
            return result;
        }

        public List<AdoptionReportTrackingViewMobileModel> GetListAdoptionReportTrackingByUserId(Guid userId, Guid petProfileId)
        {
            var reports = uow.GetService<IAdoptionReportTrackingRepository>().Get().Where(s => s.InsertedBy.Equals(userId) && s.PetProfileId.Equals(petProfileId));
            var userRepo = uow.GetService<IUserRepository>();
            var result = new List<AdoptionReportTrackingViewMobileModel>();
            foreach(var report in reports)
            {
                result.Add(new AdoptionReportTrackingViewMobileModel
                {

                    AdoptionReportTrackingId = report.AdoptionReportTrackingId,
                    InsertedAt = report.InsertedAt,
                    InsertedBy = report.InsertedBy,
                    Description = report.Description,
                    AdoptionReportTrackingImgUrl = report.AdoptionReportTrackingImgUrl,
                    PetProfileId = report.PetProfileId,
                });
            }
            return result;
        }
    }
}
