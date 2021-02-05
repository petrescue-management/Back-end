using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class AdoptionRegisterFormDomain : BaseDomain
    {
        public AdoptionRegisterFormDomain(IUnitOfWork uow) : base(uow)
        {
        }

        #region SEARCH
        public SearchReturnModel SearchAdoptionRegisterForm(SearchModel model)
        {
            var records = uow.GetService<IAdoptionRegisterFormRepository>().Get().AsQueryable();


            if (model.Status != 0)
                records = records.Where(a => a.AdoptionRegisterStatus.Equals(model.Status));

            List<AdoptionRegisterFormModel> result = records
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(a => new AdoptionRegisterFormModel
                {
                    AdoptionRegisterId = a.AdoptionRegisterId,
                    PetId = a.PetId,
                    UserName = a.UserName,
                    Phone = a.Phone,
                    Email = a.Email,
                    Job = a.Job,
                    Address = a.Address,
                    HouseType = a.HouseType,
                    FrequencyAtHome = a.FrequencyAtHome,
                    HaveChildren = a.HaveChildren,
                    ChildAge = a.ChildAge,
                    BeViolentTendencies = a.BeViolentTendencies,
                    HaveAgreement = a.HaveAgreement,
                    HavePet = a.HavePet,
                    AdoptionRegisterStatus = a.AdoptionRegisterStatus,
                    InsertedBy = a.InsertedBy,
                    InsertedAt = a.InsertedAt,
                    UpdatedBy = a.UpdatedBy,
                    UpdateAt = a.UpdateAt
                }).ToList();

            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }
        #endregion
    }
}
