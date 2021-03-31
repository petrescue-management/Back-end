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
        public int CreateNewPet(PetCreateModel model, Guid insertBy, Guid centerId)
        {
            var petRepo = uow.GetService<IPetRepository>();
            var context = uow.GetService<PetRescueContext>();
            var result = -1;
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
                            result = newPet.PetStatus;
                        }
                    }
                }catch(Exception e)
                {
                    transaction.Rollback();
                    throw (e);
                }   
            }
            return result;
            
        }
        public int UpdatePet(PetDetailModel model, Guid updateBy)
        {
            var petRepo = uow.GetService<IPetRepository>();
            var pet = petRepo.Get().FirstOrDefault(p => p.PetId == model.PetId);
            var result = -1;
            if(pet != null)
            {
                pet = petRepo.Edit(pet, model, updateBy);
                uow.saveChanges();
                result = pet.PetStatus;
            }
            return result;
            
        }
        public Pet RemovePet()
        {
            return null;
        }
        public int CreatePetFurColor(PetFurColorCreateModel model)
        {
            var result = -1;
            var petFurColorRepo = uow.GetService<IPetFurColorRepository>();
            var newPetFurColor = petFurColorRepo.Create(model);
            if(newPetFurColor != null)
            {
                uow.saveChanges();
                result = 1;
            }
            return result;
        }
        public int UpdatePetFurColor(PetFurColorUpdateModel model)
        {
            var petFurColorRepo = uow.GetService<IPetFurColorRepository>();
            var petFurColor = petFurColorRepo.Get().FirstOrDefault(p => p.PetFurColorId == model.PetFurColorId);
            var result = -1;
            if(petFurColor != null)
            {
                petFurColor = petFurColorRepo.Edit(model, petFurColor);
                uow.saveChanges();
                result = 1;
            }
            return result;
        }
        public int CreatePetBreed(PetBreedCreateModel model)
        {
            var petBreedRepo = uow.GetService<IPetBreedRepository>();
            var newPetBreed = petBreedRepo.Create(model);
            var result = -1;
            if(newPetBreed != null)
            {
                uow.saveChanges();
                result = 1;
            }
            return result;
            
            
        }
        public int UpdatePetBreed(PetBreedUpdateModel model)
        {
            var result = -1;
            var petBreedRepo = uow.GetService<IPetBreedRepository>();
            var petBreed = petBreedRepo.Get().FirstOrDefault(p => p.PetBreedId == model.PetBreedId);
            if(petBreed != null)
            {
                petBreed = petBreedRepo.Edit(model, petBreed);
                uow.saveChanges();
                result = 1;
            }
            return result;
        }
        public int CreatePetType(PetTypeCreateModel model)
        {
            var petTypeRepo = uow.GetService<IPetTypeRepository>();
            var newPetType = petTypeRepo.Create(model);
            var result = -1;
            if(newPetType != null)
            {
                result = 1;
                uow.saveChanges();
            }
            return result;
        }
        public int UpdatePetType(PetTypeUpdateModel model)
        {
            var petTypeRepo = uow.GetService<IPetTypeRepository>();
            var petType = petTypeRepo.Get().FirstOrDefault(p => p.PetTypeId == model.PetTypeId);
            var result = -1;
            if(petType != null)
            {
                petType = petTypeRepo.Edit(petType, model);
                uow.saveChanges();
                result = 1;
            }
            return result;
        }
        public Pet GetPetById(Guid petId) 
        {
            var petRepo = uow.GetService<IPetRepository>();
            return petRepo.Get().FirstOrDefault(s => s.PetId.Equals(petId));
        }
        public List<PetAdoptionRegisterFormModel> GetListPetsToBeRegisteredForAdoption(Guid centerId, PetFilter filter)
        {
            var petRepo = uow.GetService<IPetRepository>();
            var adoptionRegisterFormRepo = uow.GetService<IAdoptionRegistrationFormRepository>();
            var pets = petRepo.Get().Where(s => s.CenterId.Equals(centerId));
            pets = PetExtensions.GetAdoptionRegistrationByPet(pets, filter);
            var result = new List<PetAdoptionRegisterFormModel>();
            foreach(var pet in pets)
            {
                var count = adoptionRegisterFormRepo.Get().Where(s => s.PetId.Equals(pet.PetId) && s.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.PROCESSING).Count();
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
        public ListRegisterAdoptionOfPetViewModel GetListAdoptionRegisterFormByPetId(Guid petId)
        {
            var petRepo = uow.GetService<IPetRepository>();
            var adoptionRegisterFormRepo = uow.GetService<IAdoptionRegistrationFormRepository>();
            var forms = adoptionRegisterFormRepo.Get().Where(s => s.PetId.Equals(petId) && s.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.PROCESSING);
            var currentPet = petRepo.Get().FirstOrDefault(s => s.PetId.Equals(petId));
            var result = new ListRegisterAdoptionOfPetViewModel {
                Pet = new PetModel
                {
                    CenterId = currentPet.CenterId,
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
                    PetId = currentPet.PetId,
                    PetName = currentPet.PetNavigation.PetName,
                    PetStatus = currentPet.PetStatus,
                    Weight = currentPet.PetNavigation.Weight
                },
                AdoptionRegisterforms = new List<AdoptionRegistrationFormViewModel>()
            };
            foreach(var form in forms)
            {
                result.AdoptionRegisterforms.Add(new AdoptionRegistrationFormViewModel
                {
                    Address = form.Address,
                    AdoptionRegistrationId = form.AdoptionRegistrationId,
                    AdoptionRegistrationStatus = form.AdoptionRegistrationStatus,
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
                });
            }
            return result;
        }
        public object GetPetByTypeName()
        {
            var petRepo = uow.GetService<IPetRepository>();
            var petTypeRepo = uow.GetService<IPetTypeRepository>();
            var listPetType = petTypeRepo.Get();
            var result = new List<PetMobileViewModel>();
            foreach (var petType in listPetType) 
            {
                var temp = new List<PetModel>();
                var listPet = petRepo.Get().Where(s => s.PetNavigation.PetBreed.PetType.PetTypeName.Equals(petType.PetTypeName) && s.PetStatus == PetStatusConst.FINDINGADOPTER).ToList();
                foreach (var pet in listPet)
                {
                    temp.Add(new PetModel
                    {
                        CenterId = pet.CenterId,
                        Description = pet.PetNavigation.Description,
                        ImgUrl = pet.PetNavigation.ImageUrl,
                        IsSterilized = pet.PetNavigation.IsSterilized,
                        IsVaccinated = pet.PetNavigation.IsVaccinated,
                        PetAge = pet.PetNavigation.PetAge,
                        PetBreedId = pet.PetNavigation.PetBreedId,
                        PetBreedName = pet.PetNavigation.PetBreed.PetBreedName,
                        PetFurColorId = pet.PetNavigation.PetFurColorId,
                        PetFurColorName = pet.PetNavigation.PetFurColor.PetFurColorName,
                        PetGender = pet.PetNavigation.PetGender,
                        PetId = pet.PetId,
                        PetName = pet.PetNavigation.PetName,
                        PetStatus = pet.PetStatus,
                        Weight = pet.PetNavigation.Weight
                    });
                }
                var petMobileViewModel = new PetMobileViewModel
                {
                    ListPet = temp,
                    TypeName = petType.PetTypeName
                };
                result.Add(petMobileViewModel);
            }
            return result;
        }
    }
}
