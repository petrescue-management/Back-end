using PetRescue.Data.ConstantHelper;
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
        private readonly IAdoptionReportTrackingRepository _adoptionReportTrackingRepo;
        private readonly IUserRepository _userRepo;
        public AdoptionReportTrackingDomain(IUnitOfWork uow, 
            IAdoptionReportTrackingRepository adoptionReportTrackingRepo, 
            IUserRepository userRepo) : base(uow)
        {
            this._adoptionReportTrackingRepo = adoptionReportTrackingRepo;
            this._userRepo = userRepo;
        }
        public bool Create(AdoptionReportTrackingCreateModel model, Guid insertedBy)
        {
            var adoptionReport = _adoptionReportTrackingRepo.Create(model, insertedBy);
            if (adoptionReport != null)
            {
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
        public bool Edit(AdoptionReportTrackingUpdateModel model, Guid insertedBy)
        {
            var adoptionReport = _adoptionReportTrackingRepo.Edit(model, insertedBy);
            if (adoptionReport != null)
            {
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
        public AdoptionReportTrackingViewModel GetByAdoptionReportTrackingId(Guid reportId)
        {
            var adoptionReport = _adoptionReportTrackingRepo.Get().FirstOrDefault(s => s.AdoptionReportTrackingId.Equals(reportId));
            var result = new AdoptionReportTrackingViewModel();
            if(adoptionReport != null)
            {
                var userCreated = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(adoptionReport.InsertedBy));
                result.AdoptionReportTrackingId = adoptionReport.AdoptionReportTrackingId;
                result.InsertedAt = adoptionReport.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM);
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
            var reports = _adoptionReportTrackingRepo.Get().Where(s => s.InsertedBy.Equals(userId) && s.PetProfileId.Equals(petProfileId));
            var result = new List<AdoptionReportTrackingViewMobileModel>();
            foreach(var report in reports)
            {
                result.Add(new AdoptionReportTrackingViewMobileModel
                {
                    AdoptionReportTrackingId = report.AdoptionReportTrackingId,
                    InsertedAt = report.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
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
