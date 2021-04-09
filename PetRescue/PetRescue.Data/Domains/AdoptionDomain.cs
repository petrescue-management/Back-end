﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetRescue.Data.Domains
{
    public class AdoptionDomain : BaseDomain
    {
        public AdoptionDomain(IUnitOfWork uow) : base(uow)
        {
        }

        #region SEARCH
        public SearchReturnModel SearchAdoption(SearchModel model,string currentCenterId)
        {
            var records = uow.GetService<IAdoptionRepository>().Get().AsQueryable();
            var petProfileService = uow.GetService<IPetProfileRepository>();
            var userService = uow.GetService<IUserRepository>();

            if (model.Status != 0)
                records = records.Where(a => a.AdoptionStatus.Equals(model.Status));

            List<AdoptionModel> result = records
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Include(a => a.AdoptionRegistration)
                .ThenInclude(a => a.PetProfile)
                .Where(a => a.AdoptionRegistration.PetProfile.CenterId.Equals(Guid.Parse(currentCenterId)))
                .Select(a => new AdoptionModel
                {
                    AdoptionRegistrationId = a.AdoptionRegistrationId,
                    Owner = userService.GetUserById(a.AdoptionRegistration.InsertedBy),
                    PetProfile = petProfileService.GetPetProfileById(a.AdoptionRegistration.PetProfileId),
                    AdoptionStatus = a.AdoptionStatus,
                    AdoptedAt = a.AdoptedAt,
                    ReturnedAt = a.ReturnedAt
                }).ToList();

            if (!string.IsNullOrEmpty(model.Keyword) && !string.IsNullOrWhiteSpace(model.Keyword))
                foreach(var adoption in result)
                {
                    if(!adoption.PetProfile.PetName.Contains(model.Keyword))
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
        public AdoptionModel GetAdoptionById(Guid id)
        {
            var adoption = uow.GetService<IAdoptionRepository>().GetAdoptionById(id);
            var form = uow.GetService<IAdoptionRegistrationFormRepository>().GetAdoptionRegistrationFormById(id);
            var userService = uow.GetService<IUserRepository>();
            var petProfileService = uow.GetService<IPetProfileRepository>();
            AdoptionModel result = new AdoptionModel
            {
                AdoptionRegistrationId = id,
                Owner = userService.GetUserById(form.InsertedBy),
                PetProfile = petProfileService.GetPetProfileById(form.PetProfileId),
                AdoptionStatus = adoption.AdoptionStatus,
                AdoptedAt = adoption.AdoptedAt,
                ReturnedAt = adoption.ReturnedAt
            };
            uow.saveChanges();
            return result;
        }
        #endregion

        #region UPDATE STATUS
        public async Task<object> UpdateAdoptionStatus(UpdateStatusModel model, string path, Guid updateBy)
        {
            var adoption = uow.GetService<IAdoptionRepository>().UpdateAdoptionStatus(model, updateBy);
            var form = uow.GetService<IAdoptionRegistrationFormRepository>().GetAdoptionRegistrationFormById(model.Id);
            var userService = uow.GetService<IUserRepository>();
            var petProfileService = uow.GetService<IPetProfileRepository>();
            var adoptionFormRepo = uow.GetService<IAdoptionRegistrationFormRepository>();
            var notifcationDomain = uow.GetService<NotificationTokenDomain>();
            AdoptionModel temp = new AdoptionModel
            {
                AdoptionRegistrationId = model.Id,
                Owner = userService.GetUserById(form.InsertedBy),
                PetProfile = petProfileService.GetPetProfileById(form.PetProfileId),
                AdoptionStatus = adoption.AdoptionStatus,
                AdoptedAt = adoption.AdoptedAt,
                ReturnedAt = adoption.ReturnedAt
            };
            if (model.Status == AdoptionStatusConst.ADOPTED)
            {
                var result = new ReturnAdoptionViewModel();
                // update petProfile
                petProfileService.UpdatePetProfile(new UpdatePetProfileModel
                {
                    PetProfileId = adoption.AdoptionRegistration.PetProfileId,
                    PetStatus = PetStatusConst.ADOPTED
                }, updateBy);
                //return list reject
                result.Approve = new AdoptionFormModel
                {
                    AdoptionFormId = adoption.AdoptionRegistrationId,
                    UserId = adoption.AdoptionRegistration.InsertedBy
                };
                result.Rejects = adoptionFormRepo.Get()
                    .Where(s=>s.PetProfileId.Equals(adoption.AdoptionRegistration.PetProfileId)
                    && !s.AdoptionRegistrationId.Equals(adoption.AdoptionRegistrationId)
                    && s.AdoptionRegistrationStatus == AdoptionStatusConst.PROCESSING)
                    .Select(s=> new AdoptionFormModel 
                    {
                        AdoptionFormId = s.AdoptionRegistrationId,
                        UserId = s.InsertedBy
                    }).ToList();

                var newJson
                    = new NotificationRemindReportAfterAdopt
                    {
                        AdoptionId = temp.AdoptionRegistrationId,
                        AdoptedAt = temp.AdoptedAt,
                        OwnerId = temp.Owner.UserId,
                        Path = path
                    };
                
                var serialObject = JsonConvert.SerializeObject(newJson);

                string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "RemindReportAfterAdopt.json");

                string fileJson = File.ReadAllText(FILEPATH);

                var objJson = JObject.Parse(fileJson);

                var remindArrary = objJson.GetValue("Reminders") as JArray;

                var newNoti = JObject.Parse(serialObject);

                if (remindArrary.Count == 0)
                    remindArrary = new JArray();

                remindArrary.Add(newNoti);

                objJson["Reminders"] = remindArrary;
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(objJson, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(FILEPATH, output);

                uow.saveChanges();
                return result;
            }
            else if(model.Status == AdoptionStatusConst.DONTGET)
            {
                var result = new ReturnAdoptionViewModel();
                // update petProfile
                petProfileService.UpdatePetProfile(new UpdatePetProfileModel
                {
                    PetProfileId = adoption.AdoptionRegistration.PetProfileId,
                    PetStatus = PetStatusConst.FINDINGADOPTER
                }, updateBy);
                //update adoptionForm to reject
                adoptionFormRepo.UpdateAdoptionRegistrationFormStatus(new UpdateViewModel 
                {
                    Id = adoption.AdoptionRegistrationId,
                    Status = AdoptionRegistrationFormStatusConst.REJECTED
                }, updateBy);
                await notifcationDomain.NotificationForUser(path, adoption.AdoptionRegistration.InsertedBy,
                    ApplicationNameHelper.USER_APP,
                    new FirebaseAdmin.Messaging.Message 
                    {
                         Notification = new FirebaseAdmin.Messaging.Notification
                         {
                             Title = NotificationTitleHelper.USER_DONT_GET_PET_TITLE,
                             Body = NotificationBodyHelper.USER_DONT_GET_PET_BODY
                         }
                    });
                result.Approve = new AdoptionFormModel
                {
                    AdoptionFormId = adoption.AdoptionRegistrationId,
                    UserId = adoption.AdoptionRegistration.InsertedBy
                };
                uow.saveChanges();
                return result;
            }
            return null;
        }

        public void Remind(Guid ownerId, string path)
        {
            uow.GetService<NotificationTokenDomain>().NotificationForUserAlertAfterAdoption(path, ownerId,
                   ApplicationNameHelper.USER_APP);
        }
        #endregion
        public List<AdoptionViewModel> GetListAdoptionByCenterId(Guid centerId)
        {
            var adoptionRepo = uow.GetService<IAdoptionRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var adoptions = adoptionRepo.Get().Where(s => s.AdoptionRegistration.PetProfile.CenterId.Equals(centerId)).ToList();
            var result = new List<AdoptionViewModel>();
            foreach(var adoption in adoptions)
            {
                var user = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(adoption.InsertedBy));
                result.Add(new AdoptionViewModel {
                    AdoptedAt = adoption.AdoptedAt,
                    AdoptionRegistrationId = adoption.AdoptionRegistrationId,
                    AdoptionStatus = adoption.AdoptionStatus,
                    ReturnedAt = adoption.ReturnedAt,
                    Owner = new UserModel
                    {
                        Dob = user.UserProfile.Dob,
                        FirstName = user.UserProfile.FirstName,
                        Gender = user.UserProfile.Gender,
                        ImageUrl = user.UserProfile.ImageUrl,
                        LastName = user.UserProfile.LastName,
                        Phone = user.UserProfile.Phone,
                        UserEmail = user.UserEmail,
                        UserId = user.UserId
                    },
                    Address = adoption.AdoptionRegistration.Address,
                    Email = adoption.AdoptionRegistration.Email,
                    Job = adoption.AdoptionRegistration.Job,
                    PetBreedName = adoption.AdoptionRegistration.PetProfile.PetBreed.PetBreedName,
                    PetColorName = adoption.AdoptionRegistration.PetProfile.PetFurColor.PetFurColorName,
                    PetImgUrl = adoption.AdoptionRegistration.PetProfile.PetImgUrl,
                    PetName = adoption.AdoptionRegistration.PetProfile.PetName,
                    PetTypeName = adoption.AdoptionRegistration.PetProfile.PetBreed.PetType.PetTypeName,
                    Phone = adoption.AdoptionRegistration.Phone,
                    Username = adoption.AdoptionRegistration.UserName
                });
            }
            return result;
        }
        public List<AdoptionViewModel> GetListAdoptionByUserId(Guid userId)
        {
            var adoptionRepo = uow.GetService<IAdoptionRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var adoptions = adoptionRepo.Get().Where(s => s.AdoptionRegistration.InsertedBy.Equals(userId)).ToList();
            var result = new List<AdoptionViewModel>();
            foreach (var adoption in adoptions)
            {
                var user = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(adoption.InsertedBy));
                result.Add(new AdoptionViewModel
                {
                    AdoptedAt = adoption.AdoptedAt,
                    AdoptionRegistrationId = adoption.AdoptionRegistrationId,
                    AdoptionStatus = adoption.AdoptionStatus,
                    ReturnedAt = adoption.ReturnedAt,
                    Owner = new UserModel
                    {
                        Dob = user.UserProfile.Dob,
                        FirstName = user.UserProfile.FirstName,
                        Gender = user.UserProfile.Gender,
                        ImageUrl = user.UserProfile.ImageUrl,
                        LastName = user.UserProfile.LastName,
                        Phone = user.UserProfile.Phone,
                        UserEmail = user.UserEmail,
                        UserId = user.UserId
                    },
                    Address = adoption.AdoptionRegistration.Address,
                    Email = adoption.AdoptionRegistration.Email,
                    Job = adoption.AdoptionRegistration.Job,
                    PetBreedName = adoption.AdoptionRegistration.PetProfile.PetBreed.PetBreedName,
                    PetColorName = adoption.AdoptionRegistration.PetProfile.PetFurColor.PetFurColorName,
                    PetImgUrl = adoption.AdoptionRegistration.PetProfile.PetImgUrl,
                    PetName = adoption.AdoptionRegistration.PetProfile.PetName,
                    PetTypeName = adoption.AdoptionRegistration.PetProfile.PetBreed.PetType.PetTypeName,
                    Phone = adoption.AdoptionRegistration.Phone,
                    Username = adoption.AdoptionRegistration.UserName
                });
            }
            return result;
        }
        public AdoptionViewModel GetAdoptionByPetId(Guid petProfileId)
        {
            var adoptionRepo = uow.GetService<IAdoptionRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var petTrackingRepo = uow.GetService<IPetTrackingRepository>();
            var adoption = adoptionRepo.Get().FirstOrDefault(s => s.AdoptionRegistration.PetProfile.PetProfileId.Equals(petProfileId));
            var result = new AdoptionViewModel();
            if (adoption != null)
            {
                var petTrackings = petTrackingRepo.Get().Where(s => s.PetProfile.PetProfileId.Equals(adoption.AdoptionRegistration.PetProfileId));
                var list = new List<PetTrackingViewModel>();
                foreach(var petTracking in petTrackings)
                {
                    var trackingUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(petTracking.InsertedBy));
                    list.Add(new PetTrackingViewModel 
                    {
                        Description = petTracking.Description,
                        ImageUrl = petTracking.PetTrackingImgUrl,
                        InsertAt = petTracking.InsertedAt,
                        IsSterilized = petTracking.IsSterilized,
                        IsVaccinated = petTracking.IsVaccinated,
                        PetTrackingId = petTracking.PetTrackingId,
                        Weight = petTracking.Weight,
                        Author = trackingUser.UserProfile.LastName + " " + trackingUser.UserProfile.FirstName
                    });
                }
                var user = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(adoption.InsertedBy));
                result.AdoptedAt = adoption.AdoptedAt;
                result.AdoptionRegistrationId = adoption.AdoptionRegistrationId;
                result.AdoptionStatus = adoption.AdoptionStatus;
                result.ReturnedAt = adoption.ReturnedAt;
                result.Owner = new UserModel
                {
                    Dob = user.UserProfile.Dob,
                    FirstName = user.UserProfile.FirstName,
                    Gender = user.UserProfile.Gender,
                    ImageUrl = user.UserProfile.ImageUrl,
                    LastName = user.UserProfile.LastName,
                    Phone = user.UserProfile.Phone,
                    UserEmail = user.UserEmail,
                    UserId = user.UserId
                };
                result.Address = adoption.AdoptionRegistration.Address;
                result.Email = adoption.AdoptionRegistration.Email;
                result.Job = adoption.AdoptionRegistration.Job;
                result.PetBreedName = adoption.AdoptionRegistration.PetProfile.PetBreed.PetBreedName;
                result.PetColorName = adoption.AdoptionRegistration.PetProfile.PetFurColor.PetFurColorName;
                result.PetImgUrl = adoption.AdoptionRegistration.PetProfile.PetImgUrl;
                result.PetName = adoption.AdoptionRegistration.PetProfile.PetName;
                result.PetTypeName = adoption.AdoptionRegistration.PetProfile.PetBreed.PetType.PetTypeName;
                result.Phone = adoption.AdoptionRegistration.Phone;
                result.Username = adoption.AdoptionRegistration.UserName;
                result.PetTrackings = list;
            }
            return result;
        }
    }
}
