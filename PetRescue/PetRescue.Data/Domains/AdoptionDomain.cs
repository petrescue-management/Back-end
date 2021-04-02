using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class AdoptionDomain : BaseDomain
    {
        public AdoptionDomain(IUnitOfWork uow) : base(uow)
        {
        }

        #region SEARCH
        public SearchReturnModel SearchAdoption(SearchModel model,string currentCenterId)
        {
            var records = uow.GetService<IAdoptionRepository>().Get().AsQueryable();
            var petProfileService = uow.GetService<IPetProfileRepository>();
            var userService = uow.GetService<IUserRepository>();

            if (model.Status != 0)
                records = records.Where(a => a.AdoptionStatus.Equals(model.Status));

            List<AdoptionModel> result = records
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Include(a => a.AdoptionRegistration)
                .ThenInclude(a => a.PetProfile)
                .Where(a => a.AdoptionRegistration.PetProfile.CenterId.Equals(Guid.Parse(currentCenterId)))
                .Select(a => new AdoptionModel
                {
                    AdoptionRegistrationId = a.AdoptionRegistrationId,
                    Owner = userService.GetUserById(a.AdoptionRegistration.InsertedBy),
                    PetProfile = petProfileService.GetPetProfileById(a.AdoptionRegistration.PetProfileId),
                    AdoptionStatus = a.AdoptionStatus,
                    AdoptedAt = a.AdoptedAt,
                    ReturnedAt = a.ReturnedAt
                }).ToList();

            if (!string.IsNullOrEmpty(model.Keyword) && !string.IsNullOrWhiteSpace(model.Keyword))
                foreach(var adoption in result)
                {
                    if(!adoption.PetProfile.PetName.Contains(model.Keyword))
                        result.Remove(adoption);
                }

            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }
        #endregion

        #region GET BY ID
        public AdoptionModel GetAdoptionById(Guid id)
        {
            var adoption = uow.GetService<IAdoptionRepository>().GetAdoptionById(id);
            var form = uow.GetService<IAdoptionRegistrationFormRepository>().GetAdoptionRegistrationFormById(id);
            var userService = uow.GetService<IUserRepository>();
            var petProfileService = uow.GetService<IPetProfileRepository>();
            AdoptionModel result = new AdoptionModel
            {
                AdoptionRegistrationId = id,
                Owner = userService.GetUserById(form.InsertedBy),
                PetProfile = petProfileService.GetPetProfileById(form.PetProfileId),
                AdoptionStatus = adoption.AdoptionStatus,
                AdoptedAt = adoption.AdoptedAt,
                ReturnedAt = adoption.ReturnedAt
            };
            uow.saveChanges();
            return result;
        }
        #endregion

        #region UPDATE STATUS
        public AdoptionModel UpdateAdoptionStatus(UpdateStatusModel model)
        {
            var adoption = uow.GetService<IAdoptionRepository>().UpdateAdoptionStatus(model);
            var form = uow.GetService<IAdoptionRegistrationFormRepository>().GetAdoptionRegistrationFormById(model.Id);
            var userService = uow.GetService<IUserRepository>();
            var petProfileService = uow.GetService<IPetProfileRepository>();
            AdoptionModel result = new AdoptionModel
            {
                AdoptionRegistrationId = model.Id,
                Owner = userService.GetUserById(form.InsertedBy),
                PetProfile = petProfileService.GetPetProfileById(form.PetProfileId),
                AdoptionStatus = adoption.AdoptionStatus,
                AdoptedAt = adoption.AdoptedAt,
                ReturnedAt = adoption.ReturnedAt
            };
            uow.saveChanges();
            return result;
        }
        #endregion
    }
}
