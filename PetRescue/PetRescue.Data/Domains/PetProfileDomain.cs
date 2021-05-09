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
        private readonly IPetBreedRepository _petBreedRepo;
        private readonly IPetFurColorRepository _petFurColorRepo;
        private readonly IPetProfileRepository _petProfileRepo;
        private readonly IPetTypeRepository _petTypeRepo;
        private readonly IAdoptionRegistrationFormRepository _adoptionRegistrationFormRepo;
        private readonly IRescueDocumentRepository _rescueDocumentRepo;
        private readonly IUserRepository _userRepo;
        public PetProfileDomain(IUnitOfWork uow, 
            IPetTypeRepository petTypeRepo, 
            IPetProfileRepository petProfileRepo, 
            IPetFurColorRepository petFurColorRepo, 
            IPetBreedRepository petBreedRepo, 
            IAdoptionRegistrationFormRepository adoptionRegistrationFormRepo, 
            IRescueDocumentRepository rescueDocumentRepo, 
            IUserRepository userRepo) : base(uow)
        {
            this._petBreedRepo = petBreedRepo;
            this._petFurColorRepo = petFurColorRepo;
            this._petProfileRepo = petProfileRepo;
            this._petTypeRepo = petTypeRepo;
            this._adoptionRegistrationFormRepo = adoptionRegistrationFormRepo;
            this._rescueDocumentRepo = rescueDocumentRepo;
            this._userRepo = userRepo;
        }
        public List<PetBreedModel> GetPetBreedsByTypeId(Guid id)
        {
            return _petBreedRepo.GetPetBreedsByTypeId(id);
        }

        public PetBreedModel GetPetBreedById(Guid id)
        {
            return _petBreedRepo.GetPetBreedById(id);
        }
        public List<PetFurColorModel> GetAllPetFurColors()
        {
            return _petFurColorRepo.GetAllPetFurColors();
        }

        public PetFurColorModel GetPetFurColorById(Guid id)
        {
            return _petFurColorRepo.GetPetFurColorById(id); ;
        }

        public List<PetTypeModel> GetAllPetTypes()
        {
            return _petTypeRepo.GetAllPetTypes();
        }

        public PetTypeModel GetPetTypeById(Guid id)
        {
            return _petTypeRepo.GetPetTypeById(id);
        }
        #region SEARCH PET PROFILE
        public SearchReturnModel SearchPetProfile(SearchPetProfileModel model, Guid centerId)
        {
            var records = _petProfileRepo.Get().AsQueryable().Where(p => p.CenterId.Equals(centerId));
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
                   PetDocumentId = (Guid)p.RescueDocumentId,
                   CenterId = p.CenterId,
                   Description = p.Description,
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
            var petProfile = _petProfileRepo.CreatePetProfile(model, insertBy, centerId);
            if(petProfile != null)
            {
                _uow.SaveChanges();
                return petProfile;
            }
            return null;
        }
        #endregion
        #region UPDATE PET PROFILE
        public PetProfileModel UpdatePetProfile(UpdatePetProfileModel model, Guid updatedBy)
        {
            var petProfile = _petProfileRepo.UpdatePetProfile(model, updatedBy);
            if(petProfile != null)
            {
                _uow.SaveChanges();
                return petProfile;
            }
            return null;
        }
        #endregion

        #region GET PET PROFILE BY ID
        public PetProfileModel GetPetProfileById(Guid id)
        {
            return _petProfileRepo.GetPetProfileById(id); ;
        }
        #endregion
        public bool CreatePetFurColor(PetFurColorCreateModel model)
        {
            var newPetFurColor = _petFurColorRepo.Create(model);
            if (newPetFurColor != null)
            {
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
        public bool UpdatePetFurColor(PetFurColorUpdateModel model)
        {
            var petFurColor = _petFurColorRepo.Get().FirstOrDefault(p => p.PetFurColorId == model.PetFurColorId);
            if (petFurColor != null)
            {
                petFurColor = _petFurColorRepo.Edit(model, petFurColor);
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
        public bool CreatePetBreed(PetBreedCreateModel model)
        {
            var newPetBreed = _petBreedRepo.Create(model);
            if (newPetBreed != null)
            {
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
        public bool UpdatePetBreed(PetBreedUpdateModel model)
        {
            var petBreed = _petBreedRepo.Get().FirstOrDefault(p => p.PetBreedId == model.PetBreedId);
            if (petBreed != null)
            {
                petBreed = _petBreedRepo.Edit(model, petBreed);
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
        public bool CreatePetType(PetTypeCreateModel model)
        {
            var newPetType = _petTypeRepo.Create(model);
            if (newPetType != null)
            {
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
        public bool UpdatePetType(PetTypeUpdateModel model)
        {
            var petType = _petTypeRepo.Get().FirstOrDefault(p => p.PetTypeId == model.PetTypeId);
            if (petType != null)
            {
                petType = _petTypeRepo.Edit(petType, model);
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
        public List<PetAdoptionRegisterFormModel> GetListPetsToBeRegisteredForAdoption(Guid centerId, PetProfileFilter filter)
        {
            var petProfiles = _petProfileRepo.Get().Where(s => s.CenterId.Equals(centerId));
            petProfiles = PetProfileExtensions.GetAdoptionRegistrationByPet(petProfiles, filter);
            var result = new List<PetAdoptionRegisterFormModel>();
            foreach (var petProfile in petProfiles)
            {
                if(filter.PetStatus == PetStatusConst.FINDINGADOPTER)
                {
                    var count = _adoptionRegistrationFormRepo.Get().Where(s => s.PetProfileId.Equals(petProfile.PetProfileId) && s.AdoptionRegistrationFormStatus == AdoptionRegistrationFormStatusConst.PROCESSING).Count();
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
                            ImageUrl = petProfile.PetImgUrl,
                            UpdatedAt = petProfile.UpdatedAt?.AddHours(ConstHelper.UTC_VIETNAM)
                        });
                    }
                }
                else if(filter.PetStatus == PetStatusConst.WAITING)
                {
                    var count = _adoptionRegistrationFormRepo.Get().Where(s => s.PetProfileId.Equals(petProfile.PetProfileId) && s.AdoptionRegistrationFormStatus == AdoptionRegistrationFormStatusConst.APPROVED).Count();
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
                            ImageUrl = petProfile.PetImgUrl,
                            UpdatedAt = petProfile.UpdatedAt?.AddHours(ConstHelper.UTC_VIETNAM)
                        });
                    }
                }
                
            }
            return result;
        }
        public ListRegisterAdoptionOfPetViewModel GetListAdoptionRegisterFormByPetId(Guid petProfileId)
        {
            var forms = _adoptionRegistrationFormRepo.Get().Where(s => s.PetProfileId.Equals(petProfileId));
            var currentPet = _petProfileRepo.Get().FirstOrDefault(s => s.PetProfileId.Equals(petProfileId));
            var result = new ListRegisterAdoptionOfPetViewModel
            {
                Pet = new PetModel
                {
                    CenterId = currentPet.CenterId,
                    Description = currentPet.Description,
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
            if(currentPet.PetStatus == PetStatusConst.FINDINGADOPTER)
            {
                forms = forms.Where(s => s.AdoptionRegistrationFormStatus == AdoptionRegistrationFormStatusConst.PROCESSING);
                foreach (var form in forms)
                {
                    result.AdoptionRegisterforms.Add(new AdoptionRegistrationFormViewModel
                    {
                        Address = form.Address,
                        AdoptionRegistrationId = form.AdoptionRegistrationFormId,
                        AdoptionRegistrationStatus = form.AdoptionRegistrationFormStatus,
                        BeViolentTendencies = form.BeViolentTendencies,
                        ChildAge = form.ChildAge,
                        Email = form.Email,
                        FrequencyAtHome = form.FrequencyAtHome,
                        HaveAgreement = form.HaveAgreement,
                        HaveChildren = form.HaveChildren,
                        HavePet = form.HavePet,
                        HouseType = form.HouseType,
                        InsertedAt = form.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                        InsertedBy = form.InsertedBy,
                        Job = form.Job,
                        UpdatedAt = form.UpdatedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                        UpdatedBy = form.UpdatedBy,
                        UserName = form.UserName,
                        Phone = form.Phone,
                    });
                }
            }else if(currentPet.PetStatus == PetStatusConst.WAITING)
            {
                forms = forms.Where(s => s.AdoptionRegistrationFormStatus == AdoptionRegistrationFormStatusConst.APPROVED);
                foreach (var form in forms)
                {
                    result.AdoptionRegisterforms.Add(new AdoptionRegistrationFormViewModel
                    {
                        Address = form.Address,
                        AdoptionRegistrationId = form.AdoptionRegistrationFormId,
                        AdoptionRegistrationStatus = form.AdoptionRegistrationFormStatus,
                        BeViolentTendencies = form.BeViolentTendencies,
                        ChildAge = form.ChildAge,
                        Email = form.Email,
                        FrequencyAtHome = form.FrequencyAtHome,
                        HaveAgreement = form.HaveAgreement,
                        HaveChildren = form.HaveChildren,
                        HavePet = form.HavePet,
                        HouseType = form.HouseType,
                        InsertedAt = form.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                        InsertedBy = form.InsertedBy,
                        Job = form.Job,
                        UpdatedAt = form.UpdatedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                        UpdatedBy = form.UpdatedBy,
                        UserName = form.UserName,
                        Phone = form.Phone,
                    });
                }
            }
            return result;
        }
        public object GetPet(PetProfileFilter filter, string[] fields, int page, int limit)
        {
            var query = _petProfileRepo.Get();
            if (limit == 0)
            {
                limit = 1;
            }
            return query.GetData(filter, page, limit, fields);
        }
        #region GET PET BY TYPE NAME
        public List<GetPetByTypeNameModel> GetPetByTypeName(PetProfileFilter filter)
        {

            var petTypes = _petTypeRepo.Get();

            var result = new List<GetPetByTypeNameModel>();

            foreach (var petType in petTypes)
            {
                var listPetProfiles = new List<PetProfileModel3>();

                var records = _petProfileRepo.Get()
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
                        PetDocumentId = petProfile.RescueDocumentId,
                        PetName = petProfile.PetName,
                        PetGender = petProfile.PetGender,
                        PetAge = petProfile.PetAge,
                        PetBreedId = petProfile.PetBreedId,
                        PetBreedName = petProfile.PetBreed.PetBreedName,
                        PetFurColorId = petProfile.PetFurColorId,
                        PetFurColorName = petProfile.PetFurColor.PetFurColorName,
                        PetImgUrl = petProfile.PetImgUrl,
                        PetStatus = petProfile.PetStatus,
                        Description = petProfile.Description,
                        CenterId = petProfile.CenterId,
                        InsertedBy = petProfile.InsertedBy,
                        InsertedAt = petProfile.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
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
        public RescueDocumentViewModel GetRescueDocumentByPetId(Guid petProfileId)
        {
            

            var result = new RescueDocumentViewModel();
            var petProfile = _petProfileRepo.Get().FirstOrDefault(s => s.PetProfileId.Equals(petProfileId));
            var rescueDocument = _rescueDocumentRepo.Get().FirstOrDefault(s => s.RescueDocumentId.Equals(petProfile.RescueDocumentId));
            if (rescueDocument != null)
            {
                var currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(rescueDocument.PickerForm.InsertedBy));
                var fullName = currentUser.UserNavigation.LastName + " "+currentUser.UserNavigation.FirstName;
                var pickerForm = new PickerFormViewModel
                {
                    PickerDate = rescueDocument.PickerForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    PickerDescription = rescueDocument.PickerForm.Description,
                    PickerImageUrl = rescueDocument.PickerForm.PickerFormImgUrl,
                    PickerName = fullName,
                };
                currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(rescueDocument.FinderForm.InsertedBy));
                fullName = currentUser.UserNavigation.LastName + " " + currentUser.UserNavigation.FirstName;
                var finderForm = new FinderFormViewModel
                {
                    FinderDate = rescueDocument.FinderForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    FinderDescription = rescueDocument.FinderForm.Description,
                    FinderImageUrl = rescueDocument.FinderForm.FinderFormImgUrl,
                    FinderName = fullName,
                    Lat = rescueDocument.FinderForm.Lat,
                    Lng = rescueDocument.FinderForm.Lng,
                    FinderFormVidUrl = rescueDocument.FinderForm.FinderFormVidUrl
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
                        InsertAt = track.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                        IsSterilized = track.IsSterilized,
                        IsVaccinated = track.IsVaccinated,
                        PetTrackingId = track.PetTrackingId
                    });
                }
                result.FinderForm = finderForm;
                result.PetAttribute = rescueDocument.FinderForm.PetAttribute;
                result.PickerForm = pickerForm;
                result.PetProfile = new PetProfileModel
                {
                    CenterId = petProfile.CenterId,
                    InsertedAt = petProfile.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    InsertedBy = petProfile.InsertedBy,
                    PetAge = petProfile.PetAge,
                    PetBreedId = petProfile.PetBreedId,
                    PetBreedName = petProfile.PetBreed.PetBreedName,
                    PetDocumentId = petProfile.RescueDocumentId,
                    PetFurColorId = petProfile.PetFurColorId,
                    PetFurColorName = petProfile.PetFurColor.PetFurColorName,
                    PetGender = petProfile.PetGender,
                    PetImgUrl = petProfile.PetImgUrl,
                    PetName = petProfile.PetName,
                    Description = petProfile.Description,
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
                            InsertAt = track.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                            IsSterilized = track.IsSterilized,
                            IsVaccinated = track.IsVaccinated,
                            PetTrackingId = track.PetTrackingId
                        });
                    }
                    result.ListTracking = list;
                    result.PetProfile = new PetProfileModel
                    {
                        CenterId = petProfile.CenterId,
                        InsertedAt = petProfile.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                        InsertedBy = petProfile.InsertedBy,
                        PetAge = petProfile.PetAge,
                        PetBreedId = petProfile.PetBreedId,
                        PetBreedName = petProfile.PetBreed.PetBreedName,
                        PetDocumentId = petProfile.RescueDocumentId,
                        PetFurColorId = petProfile.PetFurColorId,
                        PetFurColorName = petProfile.PetFurColor.PetFurColorName,
                        PetGender = petProfile.PetGender,
                        PetImgUrl = petProfile.PetImgUrl,
                        PetName = petProfile.PetName,
                        Description = petProfile.Description,
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
        public object GetAllPetBreeds()
        {
            var result = new List<PetBreedDetailModel>();
            foreach(var petBreed in _petBreedRepo.Get().ToList())
            {
                result.Add(new PetBreedDetailModel 
                {
                    PetBreedId = petBreed.PetBreedId,
                    PetBreedName = petBreed.PetBreedName,
                    PetTypeName = petBreed.PetType.PetTypeName
                });
            }
            return result;
        }
    }
}
