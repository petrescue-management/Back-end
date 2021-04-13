using FirebaseAdmin.Messaging;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var petProfileService = uow.GetService<IPetProfileRepository>();
            if (model.Status != 0)
                records = records.Where(f => f.AdoptionRegistrationStatus.Equals(model.Status));

            List<AdoptionRegistrationFormModel> result = new List<AdoptionRegistrationFormModel>();
            foreach (var record in records.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize)
                  .Where(f => f.PetProfile.CenterId.Equals(Guid.Parse(currentCenterId))))
            {
                
                result.Add(new AdoptionRegistrationFormModel
                {
                    AdoptionRegistrationId = record.AdoptionRegistrationId,
                    PetProfile = petProfileService.GetPetProfileById(record.PetProfileId),
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
                    UpdatedAt = record.UpdatedAt,
                    Dob = record.Dob
                });
            }

            if (!string.IsNullOrEmpty(model.Keyword) && !string.IsNullOrWhiteSpace(model.Keyword))
                foreach (var adoption in result)
                {
                    if (!adoption.PetProfile.PetName.Contains(model.Keyword))
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
            var petProfileService = uow.GetService<IPetProfileRepository>();
            var result = new AdoptionRegistrationFormModel
            {
                AdoptionRegistrationId = form.AdoptionRegistrationId,
                PetProfile = petProfileService.GetPetProfileById(form.PetProfileId),
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
                UpdatedAt = form.UpdatedAt,
                Dob = form.Dob
            };
            return result;
        }
        #endregion
/*        #region UPDATE STATUS
        public async Task<object> UpdateAdoptionRegistrationFormStatus(UpdateViewModel model, Guid updateBy, string path)
        {
            var form = uow.GetService<IAdoptionRegistrationFormRepository>().UpdateAdoptionRegistrationFormStatus(model, updateBy);
            var petProfileService = uow.GetService<IPetProfileRepository>();
            var adoptionService = uow.GetService<IAdoptionRepository>();
            var notificationTokenDomain = uow.GetService<NotificationTokenDomain>();
            var temp = new AdoptionRegistrationFormModel
            {
                AdoptionRegistrationId = form.AdoptionRegistrationId,
                PetProfile = petProfileService.GetPetProfileById(form.PetProfileId),
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
                UpdatedAt = form.UpdatedAt
            };
            var context = uow.GetService<PetRescueContext>();
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    if (form.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.APPROVED)
                    {
                        var result = new ReturnAdoptionViewModel();
                        await notificationTokenDomain.NotificationForUserWhenAdoptionFormToBeApprove(path, form.InsertedBy);
                        adoptionService.CreateAdoption(temp);
                        var updatePetModel = new UpdatePetProfileModel
                        {
                            PetProfileId = form.PetProfileId,
                            PetStatus = PetStatusConst.WAITING
                        };
                        petProfileService.UpdatePetProfile(updatePetModel, updateBy);
                        uow.saveChanges();
                        result.Approve = new AdoptionFormModel
                        {
                            AdoptionFormId = temp.AdoptionRegistrationId,
                            UserId = temp.InsertedBy
                        };
                        transaction.Commit();
                        uow.saveChanges();
                        return result;
                    }
                    else
                    {
                        Message message = new Message
                        {
                            Notification = new Notification
                            {
                                Body = NotificationBodyHelper.REJECT_ADOPTION_FORM_TITLE,
                                Title = NotificationTitleHelper.REJECT_ADOPTION_FORM_TITLE
                            }
                        };
                        await notificationTokenDomain.NotificationForUser(path, form.InsertedBy, ApplicationNameHelper.USER_APP, message);
                        var result = new RejectAdoptionViewModel
                        {
                            Reason = model.Reason,
                            Reject = new AdoptionFormModel
                            {
                                AdoptionFormId = temp.AdoptionRegistrationId,
                                UserId = temp.InsertedBy
                            }
                        };
                        transaction.Commit();
                        uow.saveChanges();
                        return result;
                    }
                }
                catch
                {
                    transaction.Rollback();
                    return null;
                }
            }
            
        }
        #endregion*/
        #region CREATE
        public AdoptionCreateViewModel CreateAdoptionRegistrationForm(CreateAdoptionRegistrationFormModel model, Guid insertBy)
        {
            if (!IsExistedForm(insertBy,model.PetProfileId))
            {
                var form = uow.GetService<IAdoptionRegistrationFormRepository>().CreateAdoptionRegistrationForm(model, insertBy);
                var petProfile = uow.GetService<IPetProfileRepository>().Get().FirstOrDefault(s => s.PetProfileId.Equals(form.PetProfileId));
                var result = new AdoptionCreateViewModel
                {
                    AdoptionRegistrationFormId = form.AdoptionRegistrationId,
                    CenterId = petProfile.CenterId
                };
                uow.saveChanges();
                return result;
            }
            return null;
        }
        #endregion
        public List<AdoptionRegistrationFormModelWithCenter> GetListAdoptionByUserId(Guid userId)
        {
            var adoptionFormRepo = uow.GetService<IAdoptionRegistrationFormRepository>();
            var petRepo = uow.GetService<IPetProfileRepository>();
            var result = new List<AdoptionRegistrationFormModelWithCenter>();
            var listAdoptionForm = adoptionFormRepo.Get().Where(s => s.InsertedBy.Equals(userId)).ToList();
            foreach(var adoptionForm in listAdoptionForm)
            {
                result.Add(new AdoptionRegistrationFormModelWithCenter
                {
                    AdoptionRegistrationId = adoptionForm.AdoptionRegistrationId,
                    PetProfile = petRepo.GetPetProfileById2(adoptionForm.PetProfileId),
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
                    UpdatedAt = adoptionForm.UpdatedAt,
                    Dob  = adoptionForm.Dob,
                });
            }
            return result;
        }
        private bool IsExistedForm(Guid insertBy, Guid petProfileId)
        {
            var adotionFormRepo = uow.GetService<IAdoptionRegistrationFormRepository>();
            var result = adotionFormRepo.Get().FirstOrDefault(s => s.InsertedBy.Equals(insertBy) && s.PetProfileId.Equals(petProfileId) && s.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.PROCESSING);
            if(result != null)
            {
                return true;
            }
            return false;
        }
    }
}
