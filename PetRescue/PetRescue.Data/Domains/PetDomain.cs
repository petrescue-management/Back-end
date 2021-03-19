using Microsoft.EntityFrameworkCore;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Extensions;
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
    public class PetDomain : BaseDomain
    {
        public PetDomain(IUnitOfWork uow) : base(uow)
        {
        }

        public List<PetBreedModel> GetPetBreedsByTypeId(Guid id)
        {
            var breeds = uow.GetService<IPetBreedRepository>().GetPetBreedsByTypeId(id);
            return breeds;
        }

        public PetBreedModel GetPetBreedById(Guid id)
        {
            var breed = uow.GetService<IPetBreedRepository>().GetPetBreedById(id);
            return breed;
        }

        public List<PetFurColorModel> GetAllPetFurColors()
        {
            var colors = uow.GetService<IPetFurColorRepository>().GetAllPetFurColors();
            return colors;
        }

        public PetFurColorModel GetPetFurColorById(Guid id)
        {
            var color = uow.GetService<IPetFurColorRepository>().GetPetFurColorById(id);
            return color;
        }

        public List<PetTypeModel> GetAllPetTypes()
        {
            var types = uow.GetService<IPetTypeRepository>().GetAllPetTypes();
            return types;
        }

        public PetTypeModel GetPetTypeById(Guid id)
        {
            var type = uow.GetService<IPetTypeRepository>().GetPetTypeById(id);
            return type;
        }
        
        public object GetPet(PetFilter filter, string[] fields, int page, int limit)
        {
            var petRepo = uow.GetService<IPetRepository>();
            var query = petRepo.Get();
            int totalPage = 0;
            if(limit > -1)
            {
                totalPage = query.Count()/ limit;
            }
            return query.GetData(filter, page, limit, totalPage, fields);
        }
        public Pet CreateNewPet(PetCreateModel model, Guid insertBy, Guid centerId)
        {
            var petRepo = uow.GetService<IPetRepository>();
            var context = uow.GetService<PetRescueContext>();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var newPet = petRepo.Create(model, insertBy, centerId);
                    if (newPet != null)
                    {
                        //Create model for pet profile
                        var newModel = new PetDetailModel
                        {
                            PetId = newPet.PetId,
                            Description = model.Description,
                            IsSterilized = model.IsSterilized,
                            IsVaccinated = model.IsVaccinated,
                            PetAge = model.PetAge,
                            PetBreedId = model.PetBreedId,
                            PetFurColorId = model.PetFurColorId,
                            PetGender = model.PetGender,
                            PetName = model.PetName,
                            Weight = model.Weight,
                            ImageUrl = model.ImageUrl
                        };
                        var petProfileRepo = uow.GetService<IPetProfileRepository>();
                        var newProfile = petProfileRepo.Create(newModel);
                        if (newProfile != null)
                        {
                            uow.saveChanges();
                            transaction.Commit();
                            return newPet;
                        }
                    }
                }catch(Exception e)
                {
                    transaction.Rollback();
                    throw (e);
                }
                
            }
            return null;
            
        }
        public Pet UpdatePet(PetDetailModel model, Guid updateBy)
        {
            var petRepo = uow.GetService<IPetRepository>();
            var pet = petRepo.Get().FirstOrDefault(p => p.PetId == model.PetId);
            pet = petRepo.Edit(pet, model, updateBy);
            return pet;
        }
        public Pet RemovePet()
        {
            return null;
        }
        public PetFurColor CreatePetFurColor(PetFurColorCreateModel model)
        {
            var petFurColorRepo = uow.GetService<IPetFurColorRepository>();
            var newPetFurColor = petFurColorRepo.Create(model);
            return newPetFurColor;
        }
        public PetFurColor UpdatePetFurColor(PetFurColorUpdateModel model)
        {
            var petFurColorRepo = uow.GetService<IPetFurColorRepository>();
            var petFurColor = petFurColorRepo.Get().FirstOrDefault(p => p.PetFurColorId == model.PetFurColorId);
            petFurColor = petFurColorRepo.Edit(model, petFurColor);
            return petFurColorRepo.Update(petFurColor).Entity;
        }
        public PetBreed CreatePetBreed(PetBreedCreateModel model)
        {
            var petBreedRepo = uow.GetService<IPetBreedRepository>();
            var newPetBreed = petBreedRepo.Create(model);
            return newPetBreed;
        }
        public PetBreed UpdatePetBreed(PetBreedUpdateModel model)
        {
            var petBreedRepo = uow.GetService<IPetBreedRepository>();
            var petBreed = petBreedRepo.Get().FirstOrDefault(p => p.PetBreedId == model.PetBreedId);
            petBreed = petBreedRepo.Edit(model, petBreed);
            return petBreedRepo.Update(petBreed).Entity;
        }
        public PetType CreatePetType(PetTypeCreateModel model)
        {
            var petTypeRepo = uow.GetService<IPetTypeRepository>();
            var newPetType = petTypeRepo.Create(model);
            return newPetType;
        }
        public PetType UpdatePetType(PetTypeUpdateModel model)
        {
            var petTypeRepo = uow.GetService<IPetTypeRepository>();
            var petType = petTypeRepo.Get().FirstOrDefault(p => p.PetTypeId == model.PetTypeId);
            petType = petTypeRepo.Edit(petType, model);
            return petTypeRepo.Update(petType).Entity;
        }
        public Pet GetPetById(Guid petId) 
        {
            var petRepo = uow.GetService<IPetRepository>();
            return petRepo.Get().FirstOrDefault(s => s.PetId.Equals(petId));
        }
        public List<PetAdoptionRegisterFormModel> GetListPetsToBeRegisteredForAdoption(Guid centerId)
        {
            var petRepo = uow.GetService<IPetRepository>();
            var adoptionRegisterFormRepo = uow.GetService<IAdoptionRegisterFormRepository>();
            var pets = petRepo.Get().Where(s => s.CenterId.Equals(centerId));
            var result = new List<PetAdoptionRegisterFormModel>();
            foreach(var pet in pets)
            {
                var count = adoptionRegisterFormRepo.Get().Where(s => s.PetId.Equals(pet.PetId) && s.AdoptionRegisterStatus == AdoptionRegisterFormStatusConst.PROCESSING).Count();
                if(count > 0)
                {
                    result.Add(new PetAdoptionRegisterFormModel
                    {
                        Count = count,
                        PetId = pet.PetId,
                        PetName = pet.PetNavigation.PetName,
                        Age = (int)pet.PetNavigation.PetAge,
                        BreedName = pet.PetNavigation.PetBreed.PetBreedName,
                        Gender = pet.PetNavigation.PetGender,
                        ImageUrl = pet.PetNavigation.ImageUrl
                    }) ;
                }
            }
            return result;
        }
        public List<AdoptionRegisterFormModel> GetListAdoptionRegisterFormByPetId(Guid petId)
        {
            var petRepo = uow.GetService<IPetRepository>();
            var adoptionRegisterFormRepo = uow.GetService<IAdoptionRegisterFormRepository>();
            var forms = adoptionRegisterFormRepo.Get().Where(s => s.PetId.Equals(petId) && s.AdoptionRegisterStatus == AdoptionRegisterFormStatusConst.PROCESSING);
            var currentPet = petRepo.Get().FirstOrDefault(s => s.PetId.Equals(petId));
            var result = new List<AdoptionRegisterFormModel>();
            foreach(var form in forms)
            {
                result.Add(new AdoptionRegisterFormModel
                {
                    Address = form.Address,
                    AdoptionRegisterId = form.AdoptionRegisterId,
                    AdoptionRegisterStatus = form.AdoptionRegisterStatus,
                    BeViolentTendencies = form.BeViolentTendencies,
                    ChildAge = form.ChildAge,
                    Email = form.Email,
                    FrequencyAtHome = form.FrequencyAtHome,
                    HaveAgreement = form.HaveAgreement,
                    HaveChildren = form.HaveChildren,
                    HavePet = form.HavePet,
                    HouseType = form.HouseType,
                    InsertedAt = form.InsertedAt,
                    InsertedBy = form.InsertedBy,
                    Job = form.Job,
                    UpdateAt = form.UpdateAt,
                    UpdatedBy = form.UpdatedBy,
                    UserName = form.UserName,
                    Phone = form.Phone,
                    Pet = new PetModel
                    {
                        CenterId = currentPet.CenterId,
                        PetName = currentPet.PetNavigation.PetName,
                        PetId = currentPet.PetId,
                        Description = currentPet.PetNavigation.Description,
                        ImgUrl = currentPet.PetNavigation.ImageUrl,
                        IsSterilized = currentPet.PetNavigation.IsSterilized,
                        IsVaccinated = currentPet.PetNavigation.IsVaccinated,
                        PetAge = currentPet.PetNavigation.PetAge,
                        PetBreedId = currentPet.PetNavigation.PetBreedId,
                        PetBreedName = currentPet.PetNavigation.PetBreed.PetBreedName,
                        PetFurColorId = currentPet.PetNavigation.PetFurColorId,
                        PetFurColorName = currentPet.PetNavigation.PetFurColor.PetFurColorName,
                        PetGender = currentPet.PetNavigation.PetGender,
                        PetStatus = currentPet.PetStatus,
                        Weight = currentPet.PetNavigation.Weight
                    }
                });
            }
            return result;
        }
    }
}
