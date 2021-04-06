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
        public PetTrackingDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public PetTrackingViewModel Create(PetTrackingCreateModel model, Guid insertBy)
        {
            var petTrackingRepo = uow.GetService<IPetTrackingRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var result = petTrackingRepo.Create(model, insertBy);
            uow.saveChanges();
            if(result != null)
            {
                var user = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(result.InsertedBy));
                return new PetTrackingViewModel
                {
                    Description = result.Description,
                    ImageUrl = result.PetTrackingImgUrl,
                    isSterilized = (bool)result.IsSterilized,
                    isVaccinated = (bool)result.IsVaccinated,
                    Weight = result.Weight,
                    InsertAt = result.InsertedAt,
                    PetTrackingId = result.PetTrackingId,
                    Author = user.UserProfile.LastName + " " + user.UserProfile.FirstName
                };
            }
            return null;
        }
        public List<PetTrackingViewModel> GetListPetTrackingByPetProfileId(Guid petProfileId)
        {
            var petTrackingRepo = uow.GetService<IPetTrackingRepository>();
            var petTrackings = petTrackingRepo.Get().Where(s => s.PetProfileId.Equals(petProfileId)).Select(s => new PetTrackingViewModel
            {
                Description = s.Description,
                ImageUrl = s.PetTrackingImgUrl,
                InsertAt = s.InsertedAt,
                isSterilized = (bool)s.IsSterilized,
                isVaccinated = (bool)s.IsVaccinated,
                PetTrackingId = s.PetTrackingId,
                Weight =s.Weight,
            }).ToList();
            return petTrackings;
        }
        public PetTrackingViewModel GetPetTrackingById(Guid petTrackingId)
        {
            var petTrackingRepo = uow.GetService<IPetTrackingRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var petTracking = petTrackingRepo.Get().FirstOrDefault(s => s.PetTrackingId.Equals(petTrackingId));
            var result = new PetTrackingViewModel();
            if (petTracking != null)
            {
                var user = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(petTracking.InsertedBy));
                result.Description = petTracking.Description;
                result.InsertAt = petTracking.InsertedAt;
                result.isSterilized = (bool)petTracking.IsSterilized;
                result.isVaccinated = (bool)petTracking.IsVaccinated;
                result.ImageUrl = petTracking.PetTrackingImgUrl;
                result.Weight = petTracking.Weight;
                result.Author = user.UserProfile.LastName + " " + user.UserProfile.FirstName;
                result.PetTrackingId = petTracking.PetTrackingId;
            }
            return result;
        }
    }
}
