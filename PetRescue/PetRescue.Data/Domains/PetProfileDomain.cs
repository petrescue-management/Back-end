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

            if (model.PetAge != 0)
                records = records.Where(p => p.PetAge.Equals(model.PetAge));

            if (!model.PetBreedId.Equals(Guid.Empty))
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
                   PetDocumentId = (Guid)p.PetDocumentId,
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
                var count = adoptionRegisterFormRepo.Get().Where(s => s.PetProfileId.Equals(petProfile.PetProfileId) && s.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.PROCESSING).Count();
                if (count > 0)
                {
                    result.Add(new PetAdoptionRegisterFormModel
                    {
                        Count = count,
                        PetId = petProfile.PetProfileId,
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
        public ListRegisterAdoptionOfPetViewModel GetListAdoptionRegisterFormByPetId(Guid petProfileId)
        {
            var petProfileRepo = uow.GetService<IPetProfileRepository>();
            var adoptionRegisterFormRepo = uow.GetService<IAdoptionRegistrationFormRepository>();
            var forms = adoptionRegisterFormRepo.Get().Where(s => s.PetProfileId.Equals(petProfileId) && s.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.PROCESSING);
            var currentPet = petProfileRepo.Get().FirstOrDefault(s => s.PetProfileId.Equals(petProfileId));
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
                    PetId = currentPet.PetProfileId,
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
        public object GetPet(PetProfileFilter filter, string[] fields, int page, int limit)
        {
            var petProfileRepo = uow.GetService<IPetProfileRepository>();
            var query = petProfileRepo.Get();
            int totalPage = 0;
            if (limit > -1)
            {
                totalPage = query.Count() / limit;
            }
            return query.GetData(filter, page, limit, totalPage, fields);
        }
        #region GET PET BY TYPE NAME
        public List<GetPetByTypeNameModel> GetPetByTypeName(PetProfileFilter filter)
        {
            var petProfileService = uow.GetService<IPetProfileRepository>();
            var petTypeService = uow.GetService<IPetTypeRepository>();

            var petTypes = petTypeService.Get();

            var result = new List<GetPetByTypeNameModel>();

            foreach (var petType in petTypes)
            {
                var listPetProfiles = new List<PetProfileModel3>();

                var records = petProfileService.Get()
                    .Include(p => p.PetBreed)
                    .Include(p => p.PetFurColor)
                    .Where(p => p.PetBreed.PetType.PetTypeName.Equals(petType.PetTypeName) && p.PetStatus == PetStatusConst.FINDINGADOPTER);
                if (filter.PetFurColorName != null)
                    records = records.Where(p => p.PetFurColor.PetFurColorName.Equals(filter.PetFurColorName));
                if (filter.PetAge != 0)
                    records = records.Where(p => p.PetAge == filter.PetAge);
                foreach (var petProfile in records)
                {
                    var center = petProfile.Center;
                    listPetProfiles.Add(new PetProfileModel3
                    {
                        PetProfileId = petProfile.PetProfileId,
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
                        InsertedAt = petProfile.InsertedAt,
                        CenterName = center.CenterName,
                        CenterAddress = center.Address
                    });
                }

                var tmp = new GetPetByTypeNameModel
                {
                    TypeName = petType.PetTypeName,
                    Result = listPetProfiles
                };
                result.Add(tmp);
            }
            return result;
        }
        #endregion
        public PetDocumentViewModel GetDocumentPetById(Guid petProfileId)
        {
            var petDocumentRepo = uow.GetService<IPetDocumentRepository>();
            var petProfileRepo = uow.GetService<IPetProfileRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var result = new PetDocumentViewModel();
            var petProfile = petProfileRepo.Get().FirstOrDefault(s => s.PetProfileId.Equals(petProfileId));
            var petDocument = petDocumentRepo.Get().FirstOrDefault(s => s.PetDocumentId.Equals(petProfile.PetDocumentId));
            if (petDocument != null)
            {
                var currentUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(petDocument.PickerForm.InsertedBy));
                var fullName = currentUser.UserProfile.LastName + " "+currentUser.UserProfile.FirstName;
                var pickerForm = new PickerFormViewModel
                {
                    PickerDate = petDocument.PickerForm.InsertedAt,
                    PickerDescription = petDocument.PickerForm.PickerDescription,
                    PickerImageUrl = petDocument.PickerForm.PickerImageUrl,
                    PickerName = fullName,
                };
                currentUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(petDocument.FinderForm.InsertedBy));
                fullName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName;
                var finderForm = new FinderFormViewModel
                {
                    FinderDate = petDocument.FinderForm.InsertedAt,
                    FinderDescription = petDocument.FinderForm.FinderDescription,
                    FinderImageUrl = petDocument.FinderForm.FinderFormImgUrl,
                    FinderName = fullName,
                    Lat = petDocument.FinderForm.Lat,
                    Lng = petDocument.FinderForm.Lng,
                };
                var tracks = petProfile.PetTracking.ToList();
                var list = new List<PetTrackingViewModel>();
                foreach (var track in tracks)
                {
                    list.Add(new PetTrackingViewModel
                    {
                        Weight = track.Weight,
                        Description = track.Description,
                        ImageUrl = track.PetTrackingImgUrl,
                        InsertAt = track.InsertedAt,
                        IsSterilized = track.IsSterilized,
                        IsVaccinated = track.IsVaccinated,
                        PetTrackingId = track.PetTrackingId
                    });
                }
                result.FinderForm = finderForm;
                result.PetAttribute = petDocument.FinderForm.PetAttribute;
                result.PickerForm = pickerForm;
                result.PetProfile = new PetProfileModel
                {
                    CenterId = petProfile.CenterId,
                    InsertedAt = petProfile.InsertedAt,
                    InsertedBy = petProfile.InsertedBy,
                    PetAge = petProfile.PetAge,
                    PetBreedId = petProfile.PetBreedId,
                    PetBreedName = petProfile.PetBreed.PetBreedName,
                    PetDocumentId = petProfile.PetDocumentId,
                    PetFurColorId = petProfile.PetFurColorId,
                    PetFurColorName = petProfile.PetFurColor.PetFurColorName,
                    PetGender = petProfile.PetGender,
                    PetImgUrl = petProfile.PetImgUrl,
                    PetName = petProfile.PetName,
                    PetProfileDescription = petProfile.PetProfileDescription,
                    PetStatus = petProfile.PetStatus,
                    PetProfileId = petProfile.PetProfileId,
                    PetType = new PetTypeUpdateModel
                    {
                        PetTypeId = petProfile.PetBreed.PetType.PetTypeId,
                        PetTypeName = petProfile.PetBreed.PetType.PetTypeName
                    }
                };
                result.ListTracking = list;
            }
            else
            {
                var list = new List<PetTrackingViewModel>();
                if (petProfile != null)
                {
                    var tracks = petProfile.PetTracking.ToList();
                    foreach (var track in tracks)
                    {
                        list.Add(new PetTrackingViewModel
                        {
                            Weight = track.Weight,
                            Description = track.Description,
                            ImageUrl = track.PetTrackingImgUrl,
                            InsertAt = track.InsertedAt,
                            IsSterilized = track.IsSterilized,
                            IsVaccinated = track.IsVaccinated,
                            PetTrackingId = track.PetTrackingId
                        });
                    }
                    result.ListTracking = list;
                    result.PetProfile = new PetProfileModel
                    {
                        CenterId = petProfile.CenterId,
                        InsertedAt = petProfile.InsertedAt,
                        InsertedBy = petProfile.InsertedBy,
                        PetAge = petProfile.PetAge,
                        PetBreedId = petProfile.PetBreedId,
                        PetBreedName = petProfile.PetBreed.PetBreedName,
                        PetDocumentId = petProfile.PetDocumentId,
                        PetFurColorId = petProfile.PetFurColorId,
                        PetFurColorName = petProfile.PetFurColor.PetFurColorName,
                        PetGender = petProfile.PetGender,
                        PetImgUrl = petProfile.PetImgUrl,
                        PetName = petProfile.PetName,
                        PetProfileDescription = petProfile.PetProfileDescription,
                        PetStatus = petProfile.PetStatus,
                        PetProfileId = petProfile.PetProfileId,
                         PetType = new PetTypeUpdateModel
                         {
                             PetTypeId = petProfile.PetBreed.PetType.PetTypeId,
                             PetTypeName = petProfile.PetBreed.PetType.PetTypeName
                         }
                    };
                }
            }
            return result;
        }
    }
}
