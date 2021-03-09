using PetRescue.Data.Models;
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

            var pet_service = uow.GetService<IPetRepository>();
            if (model.Status != 0)
                records = records.Where(f => f.AdoptionRegisterStatus.Equals(model.Status));

            List<AdoptionRegisterFormModel> result = new List<AdoptionRegisterFormModel>();
            foreach (var record in records.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize))
            {
                
                result.Add(new AdoptionRegisterFormModel
                {
                    AdoptionRegisterId = record.AdoptionRegisterId,
                    Pet = pet_service.GetPetById(record.PetId),
                    UserName = record.UserName,
                    Phone = record.Phone,
                    Email = record.Email,
                    Job = record.Job,
                    Address = record.Address,
                    HouseType = record.HouseType,
                    FrequencyAtHome = record.FrequencyAtHome,
                    HaveChildren = record.HaveChildren,
                    ChildAge = record.ChildAge,
                    BeViolentTendencies = record.BeViolentTendencies,
                    HaveAgreement = record.HaveAgreement,
                    HavePet = record.HavePet,
                    AdoptionRegisterStatus = record.AdoptionRegisterStatus,
                    InsertedBy = record.InsertedBy,
                    InsertedAt = record.InsertedAt,
                    UpdatedBy = record.UpdatedBy,
                    UpdateAt = record.UpdateAt
                });
            }          

            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }
        #endregion

        #region GET BY ID
        public AdoptionRegisterFormModel GetAdoptionRegisterFormById(Guid id)
        {
            var form = uow.GetService<IAdoptionRegisterFormRepository>().GetAdoptionRegisterFormById(id);
            var pet_service = uow.GetService<IPetRepository>();
            var result = new AdoptionRegisterFormModel
            {
                AdoptionRegisterId = form.AdoptionRegisterId,
                Pet = pet_service.GetPetById(form.PetId),
                UserName = form.UserName,
                Phone = form.Phone,
                Email = form.Email,
                Job = form.Job,
                Address = form.Address,
                HouseType = form.HouseType,
                FrequencyAtHome = form.FrequencyAtHome,
                HaveChildren = form.HaveChildren,
                ChildAge = form.ChildAge,
                BeViolentTendencies = form.BeViolentTendencies,
                HaveAgreement = form.HaveAgreement,
                HavePet = form.HavePet,
                AdoptionRegisterStatus = form.AdoptionRegisterStatus,
                InsertedBy = form.InsertedBy,
                InsertedAt = form.InsertedAt,
                UpdatedBy = form.UpdatedBy,
                UpdateAt = form.UpdateAt
            };
            return result;
        }
        #endregion

        #region UPDATE STATUS
        public AdoptionRegisterFormModel UpdateAdoptionRegisterFormStatus(UpdateStatusModel model)
        {
            var form = uow.GetService<IAdoptionRegisterFormRepository>().UpdateAdoptionRegisterFormStatus(model);
            var pet_service = uow.GetService<IPetRepository>();
            var result = new AdoptionRegisterFormModel
            {
                AdoptionRegisterId = form.AdoptionRegisterId,
                Pet = pet_service.GetPetById(form.PetId),
                UserName = form.UserName,
                Phone = form.Phone,
                Email = form.Email,
                Job = form.Job,
                Address = form.Address,
                HouseType = form.HouseType,
                FrequencyAtHome = form.FrequencyAtHome,
                HaveChildren = form.HaveChildren,
                ChildAge = form.ChildAge,
                BeViolentTendencies = form.BeViolentTendencies,
                HaveAgreement = form.HaveAgreement,
                HavePet = form.HavePet,
                AdoptionRegisterStatus = form.AdoptionRegisterStatus,
                InsertedBy = form.InsertedBy,
                InsertedAt = form.InsertedAt,
                UpdatedBy = form.UpdatedBy,
                UpdateAt = form.UpdateAt
            };
            return result;
        }
        #endregion
    }
}
