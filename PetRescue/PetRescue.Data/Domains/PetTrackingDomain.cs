using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
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
            var result = petTrackingRepo.Create(model, insertBy);
            uow.saveChanges();
            if(result != null)
            {
                return new PetTrackingViewModel
                {
                    Description = result.Description,
                    ImageUrl = result.PetTrackingImgUrl,
                    isSterilized = (bool)result.IsSterilized,
                    isVaccinated = (bool)result.IsVaccinated,
                    Weight = result.Weight,
                    InsertAt = result.InsertedAt,
                    PetTrackingId = result.PetTrackingId
                };
            }
            return null;
        }
    }
}
