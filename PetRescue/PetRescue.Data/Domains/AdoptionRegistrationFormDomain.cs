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
    public class AdoptionRegistrationFormDomain : BaseDomain
    {
        public AdoptionRegistrationFormDomain(IUnitOfWork uow) : base(uow)
        {
        }

        #region SEARCH
        public SearchReturnModel SearchAdoptionRegistrationForm(SearchModel model, string currentCenterId)
        {
            var records = uow.GetService<IAdoptionRegistrationFormRepository>().Get().AsQueryable();

            var pet_service = uow.GetService<IPetRepository>();
            if (model.Status != 0)
                records = records.Where(f => f.AdoptionRegistrationStatus.Equals(model.Status));

            List<AdoptionRegistrationFormModel> result = new List<AdoptionRegistrationFormModel>();
            foreach (var record in records.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize)
                  .Where(f => f.Pet.CenterId.Equals(Guid.Parse(currentCenterId))))
            {
                
                result.Add(new AdoptionRegistrationFormModel
                {
                    AdoptionRegistrationId = record.AdoptionRegistrationId,
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
                    AdoptionRegistrationStatus = record.AdoptionRegistrationStatus,
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
        public AdoptionRegistrationFormModel GetAdoptionRegistrationFormById(Guid id)
        {
            var form = uow.GetService<IAdoptionRegistrationFormRepository>().GetAdoptionRegistrationFormById(id);
            var pet_service = uow.GetService<IPetRepository>();
            var result = new AdoptionRegistrationFormModel
            {
                AdoptionRegistrationId = form.AdoptionRegistrationId,
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
                AdoptionRegistrationStatus = form.AdoptionRegistrationStatus,
                InsertedBy = form.InsertedBy,
                InsertedAt = form.InsertedAt,
                UpdatedBy = form.UpdatedBy,
                UpdateAt = form.UpdateAt
            };
            return result;
        }
        #endregion

        #region UPDATE STATUS
        public AdoptionRegistrationFormModel UpdateAdoptionRegistrationFormStatus(UpdateStatusModel model, Guid updateBy)
        {
            var form = uow.GetService<IAdoptionRegistrationFormRepository>().UpdateAdoptionRegistrationFormStatus(model, updateBy);
            var pet_service = uow.GetService<IPetRepository>();
            var adoption_service = uow.GetService<IAdoptionRepository>();
            var adoptionRegisterFormRepo = uow.GetService<IAdoptionRegistrationFormRepository>();
            var result = new AdoptionRegistrationFormModel
            {
                AdoptionRegistrationId = form.AdoptionRegistrationId,
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
                AdoptionRegistrationStatus = form.AdoptionRegistrationStatus,
                InsertedBy = form.InsertedBy,
                InsertedAt = form.InsertedAt,
                UpdatedBy = form.UpdatedBy,
                UpdateAt = form.UpdateAt
            };
            var currentPet = pet_service.Get().FirstOrDefault(s => s.PetId.Equals(form.PetId));
            if (form.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.APPROVED)
            {
                adoption_service.CreateAdoption(result);
                currentPet.PetStatus = PetStatusConst.ADOPTED;
                pet_service.Update(currentPet);
                var listAdoptionForm = adoptionRegisterFormRepo.Get().Where(s => s.PetId.Equals(form.PetId) && s.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.PROCESSING);
                foreach(var adoptionForm in listAdoptionForm)
                {
                    adoptionForm.AdoptionRegistrationStatus = AdoptionRegistrationFormStatusConst.REJECTED;
                    adoptionRegisterFormRepo.Update(adoptionForm);
                }
            }
            uow.saveChanges();
            return result;
        }
        #endregion

        #region CREATE
        public AdoptionRegistrationFormModel CreateAdoptionRegistrationForm(CreateAdoptionRegistrationFormModel model, Guid insertBy)
        {
            var form = uow.GetService<IAdoptionRegistrationFormRepository>().CreateAdoptionRegistrationForm(model, insertBy);
            var pet_service = uow.GetService<IPetRepository>();
            var result = new AdoptionRegistrationFormModel
            {
                AdoptionRegistrationId = form.AdoptionRegistrationId,
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
                AdoptionRegistrationStatus = form.AdoptionRegistrationStatus,
                InsertedBy = form.InsertedBy,
                InsertedAt = form.InsertedAt,
                UpdatedBy = form.UpdatedBy,
                UpdateAt = form.UpdateAt
            };
            uow.saveChanges();
            return result;
        }
            #endregion
        public List<AdoptionRegistrationFormModel> GetListAdoptionByUserId(Guid userId)
        {
            var adoptionFormRepo = uow.GetService<IAdoptionRegistrationFormRepository>();
            var petRepo = uow.GetService<IPetRepository>();
            var result = new List<AdoptionRegistrationFormModel>();
            var listAdoptionForm = adoptionFormRepo.Get().Where(s => s.InsertedBy.Equals(userId)).ToList();
            foreach(var adoptionForm in listAdoptionForm)
            {
                result.Add(new AdoptionRegistrationFormModel
                {
                    AdoptionRegistrationId = adoptionForm.AdoptionRegistrationId,
                    Pet = petRepo.GetPetById(adoptionForm.PetId),
                    UserName = adoptionForm.UserName,
                    Phone = adoptionForm.Phone,
                    Email = adoptionForm.Email,
                    Job = adoptionForm.Job,
                    Address = adoptionForm.Address,
                    HouseType = adoptionForm.HouseType,
                    FrequencyAtHome = adoptionForm.FrequencyAtHome,
                    HaveChildren = adoptionForm.HaveChildren,
                    ChildAge = adoptionForm.ChildAge,
                    BeViolentTendencies = adoptionForm.BeViolentTendencies,
                    HaveAgreement = adoptionForm.HaveAgreement,
                    HavePet = adoptionForm.HavePet,
                    AdoptionRegistrationStatus = adoptionForm.AdoptionRegistrationStatus,
                    InsertedBy = adoptionForm.InsertedBy,
                    InsertedAt = adoptionForm.InsertedAt,
                    UpdatedBy = adoptionForm.UpdatedBy,
                    UpdateAt = adoptionForm.UpdateAt
                });
            }
            return result;
        }
        }
}
