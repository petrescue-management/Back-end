using PetRescue.Data.ConstantHelper;
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
        public SearchReturnModel SearchAdoptionRegisterForm(SearchModel model, string currentCenterId)
        {
            var records = uow.GetService<IAdoptionRegisterFormRepository>().Get().AsQueryable();

            var pet_service = uow.GetService<IPetRepository>();
            if (model.Status != 0)
                records = records.Where(f => f.AdoptionRegisterStatus.Equals(model.Status));

            List<AdoptionRegisterFormModel> result = new List<AdoptionRegisterFormModel>();
            foreach (var record in records.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize)
                  .Where(f => f.Pet.CenterId.Equals(Guid.Parse(currentCenterId))))
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

            if (!string.IsNullOrEmpty(model.Keyword) && !string.IsNullOrWhiteSpace(model.Keyword))
                foreach (var adoption in result)
                {
                    if (!adoption.Pet.PetName.Contains(model.Keyword))
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
        public AdoptionRegisterFormModel UpdateAdoptionRegisterFormStatus(UpdateStatusModel model, Guid updateBy)
        {
            var form = uow.GetService<IAdoptionRegisterFormRepository>().UpdateAdoptionRegisterFormStatus(model, updateBy);
            var pet_service = uow.GetService<IPetRepository>();
            var adoption_service = uow.GetService<IAdoptionRepository>();
            var adoptionRegisterFormRepo = uow.GetService<IAdoptionRegisterFormRepository>();
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
            var currentPet = pet_service.Get().FirstOrDefault(s => s.PetId.Equals(form.PetId));
            if (form.AdoptionRegisterStatus == AdoptionRegisterFormStatusConst.APPROVED)
            {
                adoption_service.CreateAdoption(result);
                currentPet.PetStatus = PetStatusConst.ADOPTED;
                pet_service.Update(currentPet);
                var listAdoptionForm = adoptionRegisterFormRepo.Get().Where(s => s.PetId.Equals(form.PetId) && s.AdoptionRegisterStatus == AdoptionRegisterFormStatusConst.PROCESSING);
                foreach(var adoptionForm in listAdoptionForm)
                {
                    adoptionForm.AdoptionRegisterStatus = AdoptionRegisterFormStatusConst.REJECTED;
                    adoptionRegisterFormRepo.Update(adoptionForm);
                }
            }
            return result;
        }
        #endregion

        #region CREATE
        public AdoptionRegisterFormModel CreateAdoptionRegisterForm(CreateAdoptionRegisterFormModel model, Guid insertBy)
        {
            var form = uow.GetService<IAdoptionRegisterFormRepository>().CreateAdoptionRegistertionForm(model, insertBy);
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
