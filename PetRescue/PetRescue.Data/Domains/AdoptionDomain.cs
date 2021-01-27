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
        public SearchReturnModel SearchAdoption(SearchModel model)
        {
            var records = uow.GetService<IAdoptionRepository>().Get().AsQueryable();

            if (!string.IsNullOrEmpty(model.Keyword) && !string.IsNullOrWhiteSpace(model.Keyword))
                records = records.Where(a => a.AdoptionRegister.UserName.Contains(model.Keyword));

            List<AdoptionModel> result = records
                .Skip((model.PageIndex - 1) * 10)
                .Take(10)
                .Include(a => a.AdoptionRegister)
                .Select(a => new AdoptionModel
                {
                    AdoptionRegisterId = a.AdoptionRegisterId,
                    OwnerId = a.AdoptionRegister.InsertedBy,
                    OwnerName = a.AdoptionRegister.UserName,
                    PetId = a.AdoptionRegister.PetId,
                    AdoptionStatus = a.AdoptionStatus,
                    AdoptedAt = a.AdoptedAt,
                    ReturnedAt = a.ReturnedAt
                }).ToList();

            //call service.getPetById -> to find PetName By result.PetId

            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }
        #endregion

        public AdoptionModel GetAdoptionById(Guid id)
        {
            var adoption = uow.GetService<IAdoptionRepository>().GetAdoptionById(id);
            return adoption;
        }

        public AdoptionModel UpdateAdoption(UpdateStatusModel model)
        {
            var adoption = uow.GetService<IAdoptionRepository>().UpdateAdoption(model);
            return adoption;
        }
    }
}
