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
using static PetRescue.Data.ViewModels.PetProfileModel;

namespace PetRescue.Data.Domains
{
    public class PetProfileDomain : BaseDomain
    {
        public PetProfileDomain(IUnitOfWork uow) : base(uow)
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

        #region SEARCH PET PROFILE
        public SearchReturnModel SearchPetProfile(SearchPetProfileModel model, Guid centerId)
        {
            var records = uow.GetService<IPetProfileRepository>().Get().AsQueryable().Where(p => p.CenterId.Equals(centerId));

            records.Include(p => p.PetFurColor)
                .Include(p => p.PetBreed)
                .ThenInclude(p => p.PetType)
                .Where(p => p.PetBreed.PetType.PetTypeId.Equals(model.PetTypeId));

            if (model.PetGender != 0)
                records = records.Where(p => p.PetGender.Equals(model.PetGender));

            if(model.PetAge != 0)
                records = records.Where(p => p.PetAge.Equals(model.PetAge));

            if(!model.PetBreedId.Equals(Guid.Empty))
                records = records.Where(p => p.PetBreedId.Equals(model.PetBreedId));

            if (!model.PetFurColorId.Equals(Guid.Empty))
                records = records.Where(p => p.PetFurColorId.Equals(model.PetFurColorId));

            if (model.PetStatus != 0)
                records = records.Where(p => p.PetStatus.Equals(model.PetStatus));

            List<PetProfileModel> result = records
               .Skip((model.PageIndex - 1) * model.PageSize)
               .Take(model.PageSize)
               .Select(p => new PetProfileModel
               {
                   PetDocumentId = p.PetDocumentId,
                   CenterId = p.CenterId,
                   PetProfileDescription = p.PetProfileDescription,
                   PetAge = p.PetAge,
                   PetBreedId = p.PetBreedId,
                   PetBreedName = p.PetBreed.PetBreedName,
                   PetFurColorId = p.PetFurColorId,
                   PetFurColorName = p.PetFurColor.PetFurColorName,
                   PetGender = p.PetGender,
                   PetName = p.PetName,
                   PetStatus = p.PetStatus,
                   PetImgUrl = p.PetImgUrl
               }).ToList();
            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }
            #endregion

        #region CREATE PET PROFILE
            public PetProfileModel CreatePetProfile(CreatePetProfileModel model, Guid insertBy, Guid centerId)
        {
            var petProfile = uow.GetService<IPetProfileRepository>().CreatePetProfile(model, insertBy, centerId);
            uow.saveChanges();
            return petProfile;

        }
        #endregion

        #region UPDATE PET PROFILE
        public PetProfileModel UpdatePetProfile(UpdatePetProfileModel model, Guid updatedBy)
        { 
            var petProfile = uow.GetService<IPetProfileRepository>().UpdatePetProfile(model, updatedBy);
            uow.saveChanges();
            return petProfile;

        }
        #endregion

        #region GET PET PROFILE BY ID
        public PetProfileModel GetPetProfileById(Guid id)
        {
            var petProfile = uow.GetService<IPetProfileRepository>().GetPetProfileById(id);
            return petProfile;
        }
        #endregion

        public int CreatePetFurColor(PetFurColorCreateModel model)
        {
            var result = -1;
            var petFurColorRepo = uow.GetService<IPetFurColorRepository>();
            var newPetFurColor = petFurColorRepo.Create(model);
            if (newPetFurColor != null)
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
            if (petFurColor != null)
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
            if (newPetBreed != null)
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
            if (petBreed != null)
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
            if (newPetType != null)
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
            if (petType != null)
            {
                petType = petTypeRepo.Edit(petType, model);
                uow.saveChanges();
                result = 1;
            }
            return result;
        }
     public List<PetAdoptionRegisterFormModel> GetListPetsToBeRegisteredForAdoption(Guid centerId, PetProfileFilter filter)
        {
            var petProfileRepo = uow.GetService<IPetProfileRepository>();
            var adoptionRegisterFormRepo = uow.GetService<IAdoptionRegistrationFormRepository>();
            var petProfiles = petProfileRepo.Get().Where(s => s.CenterId.Equals(centerId));
            petProfiles = PetProfileExtensions.GetAdoptionRegistrationByPet(petProfiles, filter);
            var result = new List<PetAdoptionRegisterFormModel>();
            foreach (var petProfile in petProfiles)
            {
                var count = adoptionRegisterFormRepo.Get().Where(s => s.PetDocumentId.Equals(petProfile.PetDocumentId) && s.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.PROCESSING).Count();
                if (count > 0)
                {
                    result.Add(new PetAdoptionRegisterFormModel
                    {
                        Count = count,
                        PetId = petProfile.PetDocumentId,
                        PetName = petProfile.PetName,
                        Age = (int)petProfile.PetAge,
                        BreedName = petProfile.PetBreed.PetBreedName,
                        Gender = petProfile.PetGender,
                        ImageUrl = petProfile.PetImgUrl
                    });
                }
            }
            return result;
        }
        public ListRegisterAdoptionOfPetViewModel GetListAdoptionRegisterFormByPetId(Guid petDocumentId)
        {
            var petProfileRepo = uow.GetService<IPetProfileRepository>();
            var adoptionRegisterFormRepo = uow.GetService<IAdoptionRegistrationFormRepository>();
            var forms = adoptionRegisterFormRepo.Get().Where(s => s.PetDocumentId.Equals(petDocumentId) && s.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.PROCESSING);
            var currentPet = petProfileRepo.Get().FirstOrDefault(s => s.PetDocumentId.Equals(petDocumentId));
            var result = new ListRegisterAdoptionOfPetViewModel
            {
                Pet = new PetModel
                {
                    CenterId = currentPet.CenterId,
                    Description = currentPet.PetProfileDescription,
                    ImgUrl = currentPet.PetImgUrl,
                    PetAge = currentPet.PetAge,
                    PetBreedId = currentPet.PetBreedId,
                    PetBreedName = currentPet.PetBreed.PetBreedName,
                    PetFurColorId = currentPet.PetFurColorId,
                    PetFurColorName = currentPet.PetFurColor.PetFurColorName,
                    PetGender = currentPet.PetGender,
                    PetId = currentPet.PetDocumentId,
                    PetName = currentPet.PetName,
                    PetStatus = currentPet.PetStatus,
                },
                AdoptionRegisterforms = new List<AdoptionRegistrationFormViewModel>()
            };
            foreach (var form in forms)
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
                    UpdatedAt = form.UpdatedAt,
                    UpdatedBy = form.UpdatedBy,
                    UserName = form.UserName,
                    Phone = form.Phone,
                });
            }
            return result;
        }

        #region GET PET BY TYPE NAME
        public List<GetPetByTypeNameModel> GetPetByTypeName()
        {
            var petProfileService = uow.GetService<IPetProfileRepository>();
            var petTypeService = uow.GetService<IPetTypeRepository>();

            var petTypes = petTypeService.Get();

            var result = new List<GetPetByTypeNameModel>();

            foreach (var petType in petTypes)
            {
                var listPetProfiles = new List<PetProfileModel>();

                var records = petProfileService.Get()
                    .Include(p => p.PetBreed)
                    .Include(p => p.PetFurColor)
                    .Where(p => p.PetBreed.PetType.PetTypeName.Equals(petType.PetTypeName) && p.PetStatus == PetStatusConst.FINDINGADOPTER).ToList();

                foreach (var petProfile in records)
                {
                    listPetProfiles.Add(new PetProfileModel
                    {
                        PetDocumentId = petProfile.PetDocumentId,
                        PetName = petProfile.PetName,
                        PetGender = petProfile.PetGender,
                        PetAge = petProfile.PetAge,
                        PetBreedId = petProfile.PetBreedId,
                        PetBreedName = petProfile.PetBreed.PetBreedName,
                        PetFurColorId = petProfile.PetFurColorId,
                        PetFurColorName = petProfile.PetFurColor.PetFurColorName,
                        PetImgUrl = petProfile.PetImgUrl,
                        PetStatus = petProfile.PetStatus,
                        PetProfileDescription = petProfile.PetProfileDescription,
                        CenterId = petProfile.CenterId,
                        InsertedBy = petProfile.InsertedBy,
                        InsertedAt = petProfile.InsertedAt
                    });
                }

                var tmp = new GetPetByTypeNameModel
                {
                    TypeName = petType.PetTypeName,
                    result = listPetProfiles
                };
                result.Add(tmp);
            }
            return result;
        }
        #endregion
    }
}
