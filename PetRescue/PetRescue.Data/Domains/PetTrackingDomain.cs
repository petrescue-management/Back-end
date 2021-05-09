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
    public class PetTrackingDomain : BaseDomain
    {
        private readonly IPetTrackingRepository _petTrackingRepo;
        private readonly IUserRepository _userRepo;

        public PetTrackingDomain(IUnitOfWork uow, 
            IPetTrackingRepository petTrackingRepo, 
            IUserRepository userRepo) : base(uow)
        {
            this._petTrackingRepo = petTrackingRepo;
            this._userRepo = userRepo;
        }
        public PetTrackingViewModel Create(PetTrackingCreateModel model, Guid insertBy)
        {
            var result = _petTrackingRepo.Create(model, insertBy);
            _uow.SaveChanges();
            if(result != null)
            {
                var user = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(result.InsertedBy));
                return new PetTrackingViewModel
                {
                    Description = result.Description,
                    ImageUrl = result.PetTrackingImgUrl,
                    IsSterilized = result.IsSterilized,
                    IsVaccinated = result.IsVaccinated,
                    Weight = result.Weight,
                    InsertAt = result.InsertedAt.AddHours(ConstHelper.UTC_VIETNAM),
                    PetTrackingId = result.PetTrackingId,
                    Author = user.UserProfile.LastName + " " + user.UserProfile.FirstName
                };
            }
            return null;
        }
        public List<PetTrackingViewModel> GetListPetTrackingByPetProfileId(Guid petProfileId)
        {
            var petTrackings = _petTrackingRepo.Get().Where(s => s.PetProfileId.Equals(petProfileId)).Select(s => new PetTrackingViewModel
            {
                Description = s.Description,
                ImageUrl = s.PetTrackingImgUrl,
                InsertAt = s.InsertedAt.AddHours(ConstHelper.UTC_VIETNAM),
                IsSterilized = s.IsSterilized,
                IsVaccinated = s.IsVaccinated,
                PetTrackingId = s.PetTrackingId,
                Weight =s.Weight,
            }).ToList();
            return petTrackings;
        }
        public PetTrackingViewModel GetPetTrackingById(Guid petTrackingId)
        {
            var petTracking = _petTrackingRepo.Get().FirstOrDefault(s => s.PetTrackingId.Equals(petTrackingId));
            var result = new PetTrackingViewModel();
            if (petTracking != null)
            {
                var user = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(petTracking.InsertedBy));
                result.Description = petTracking.Description;
                result.InsertAt = petTracking.InsertedAt.AddHours(ConstHelper.UTC_VIETNAM);
                result.IsSterilized = petTracking.IsSterilized;
                result.IsVaccinated = petTracking.IsVaccinated;
                result.ImageUrl = petTracking.PetTrackingImgUrl;
                result.Weight = petTracking.Weight;
                result.Author = user.UserProfile.LastName + " " + user.UserProfile.FirstName;
                result.PetTrackingId = petTracking.PetTrackingId;
            }
            return result;
        }

        public bool CreatePetTrackingByUser(CreatePetTrackingByUserModel model, Guid insertedBy)
        {
            var result = _petTrackingRepo.CreatePetTrackingByUser(model, insertedBy);
            if (result != null)
            {
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
