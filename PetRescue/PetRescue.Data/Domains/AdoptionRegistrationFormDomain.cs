using FirebaseAdmin.Messaging;
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
using System.Threading.Tasks;

namespace PetRescue.Data.Domains
{
    public class AdoptionRegistrationFormDomain : BaseDomain
    {
        private readonly IAdoptionRegistrationFormRepository _adotionRegistrationFormRepo;
        private readonly IPetProfileRepository _petProfileRepo;
        private readonly DbContext _context;
        public AdoptionRegistrationFormDomain(IUnitOfWork uow, 
            IAdoptionRegistrationFormRepository adoptionRegistrationFromRepo, 
            IPetProfileRepository petProfileRepo, 
            DbContext context) : base(uow)
        {
            this._adotionRegistrationFormRepo = adoptionRegistrationFromRepo;
            this._petProfileRepo = petProfileRepo;
            this._context = context;
        }

        #region SEARCH
        public SearchReturnModel SearchAdoptionRegistrationForm(SearchModel model, string currentCenterId)
        {
            var records = _adotionRegistrationFormRepo.Get().AsQueryable();
            if (model.Status != 0)
                records = records.Where(f => f.AdoptionRegistrationStatus.Equals(model.Status));

            List<AdoptionRegistrationFormModel> result = new List<AdoptionRegistrationFormModel>();
            foreach (var record in records.Skip((model.PageIndex - 1) * model.PageSize).Take(model.PageSize)
                  .Where(f => f.PetProfile.CenterId.Equals(Guid.Parse(currentCenterId))))
            {
                
                result.Add(new AdoptionRegistrationFormModel
                {
                    AdoptionRegistrationId = record.AdoptionRegistrationId,
                    PetProfile = _petProfileRepo.GetPetProfileById(record.PetProfileId),
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
        public object GetAdoptionRegistrationFormById(Guid id)
        {
            var form = _adotionRegistrationFormRepo.GetAdoptionRegistrationFormById(id);
            var petProfile = new PetProfileMobile
            {
                CenterId = form.PetProfile.CenterId,
                PetProfileId = form.PetProfileId,
                InsertedAt = form.InsertedAt.AddHours(ConstHelper.UTC_VIETNAM),
                InsertedBy = form.InsertedBy,
                PetAge = form.PetProfile.PetAge,
                PetBreedId = form.PetProfile.PetBreedId,
                PetBreedName = form.PetProfile.PetBreed.PetBreedName,
                PetDocumentId = form.PetProfile.RescueDocumentId,
                PetFurColorId = form.PetProfile.PetFurColorId,
                PetFurColorName = form.PetProfile.PetFurColor.PetFurColorName,
                PetGender = form.PetProfile.PetGender,
                PetImgUrl = form.PetProfile.PetImgUrl,
                PetName = form.PetProfile.PetName,
                PetProfileDescription = form.PetProfile.PetProfileDescription,
                PetStatus = form.PetProfile.PetStatus,
                CenterAddress = form.PetProfile.Center.Address,
                CenterName = form.PetProfile.Center.CenterName
            };
            var result = new AdoptionRegistrationFormModelMobile
            {
                AdoptionRegistrationId = form.AdoptionRegistrationId,
                PetProfile = petProfile,
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
                InsertedAt = form.InsertedAt.AddHours(ConstHelper.UTC_VIETNAM),
                UpdatedBy = form.UpdatedBy,
                UpdatedAt = form.UpdatedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                Dob = form.Dob,
            };
            return result;
        }
        #endregion
        #region UPDATE STATUS
        public async Task<object> UpdateAdoptionRegistrationFormStatus(UpdateViewModel model, Guid updateBy, string path)
        {
            var form = _adotionRegistrationFormRepo.UpdateAdoptionRegistrationFormStatus(model, updateBy);
            var temp = new AdoptionRegistrationFormModel
            {
                AdoptionRegistrationId = form.AdoptionRegistrationId,
                PetProfile = _petProfileRepo.GetPetProfileById(form.PetProfileId),
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
                InsertedAt = form.InsertedAt.AddHours(ConstHelper.UTC_VIETNAM),
                UpdatedBy = form.UpdatedBy,
                UpdatedAt = form.UpdatedAt?.AddHours(ConstHelper.UTC_VIETNAM)
            };
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (form.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.APPROVED)
                    {
                        var result = new ReturnAdoptionViewModel();
                        await _uow.GetService<NotificationTokenDomain>().NotificationForUserWhenAdoptionFormToBeApprove(path, form.InsertedBy);
                        var updatePetModel = new UpdatePetProfileModel
                        {
                            PetProfileId = form.PetProfileId,
                            PetStatus = PetStatusConst.WAITING
                        };
                        _petProfileRepo.UpdatePetProfile(updatePetModel, updateBy);
                        _uow.SaveChanges();
                        ///Send mail
                        //var centerModel = new CenterViewModel
                        //{
                        //    Address = form.PetProfile.Center.Address,
                        //    CenterName = form.PetProfile.Center.CenterName,
                        //    Email = form.PetProfile.Center.CenterNavigation.Phone,
                        //    Phone = form.PetProfile.Center.Phone
                        //};
                        //MailArguments mailArguments = MailFormat.MailModel(form.Email, MailConstant.ApproveAdoption(centerModel, form.PetProfile.PetName, form.UserName), MailConstant.APPROVE_ADOPTION);
                        //MailExtensions.SendBySendGrid(mailArguments, null, null);
                        result.Approve = new AdoptionFormModel
                        {
                            AdoptionFormId = temp.AdoptionRegistrationId,
                            UserId = temp.InsertedBy
                        };
                        transaction.Commit();
                        return result;
                    }
                    else if (form.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.REJECTED)
                    {
                        Message message = new Message
                        {
                            Notification = new Notification
                            {
                                Body = NotificationBodyHelper.REJECT_ADOPTION_FORM_BODY,
                                Title = NotificationTitleHelper.REJECT_ADOPTION_FORM_TITLE
                            }
                        };
                        await _uow.GetService<NotificationTokenDomain>().NotificationForUser(path, form.InsertedBy, ApplicationNameHelper.USER_APP, message);
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
                        _uow.SaveChanges();
                        return result;
                    }
                    return null;
                }
                catch
                {
                    transaction.Rollback();
                    return null;
                }
            }
        }
        #endregion
        #region CREATE
        public AdoptionCreateViewModel CreateAdoptionRegistrationForm(CreateAdoptionRegistrationFormModel model, Guid insertBy)
        {
            var form = _adotionRegistrationFormRepo.CreateAdoptionRegistrationForm(model, insertBy);
            var petProfile = _petProfileRepo.Get().FirstOrDefault(s => s.PetProfileId.Equals(form.PetProfileId));
            var result = new AdoptionCreateViewModel
            {
                AdoptionRegistrationFormId = form.AdoptionRegistrationId,
                CenterId = petProfile.CenterId
            };
            _uow.SaveChanges();
            if(result != null)
            {
                return result;
            }
            return null;

        }
        public bool CheckIsExistedForm(Guid insertedBy, Guid petProfileId)
        {
            return IsExistedForm(insertedBy, petProfileId);
        }
        #endregion
        public List<AdoptionRegistrationFormModelWithCenter> GetListAdoptionByUserId(Guid userId)
        {
            var result = new List<AdoptionRegistrationFormModelWithCenter>();
            var listAdoptionForm = _adotionRegistrationFormRepo.Get().Where(s => s.InsertedBy.Equals(userId)).ToList();
            foreach(var adoptionForm in listAdoptionForm)
            {
                result.Add(new AdoptionRegistrationFormModelWithCenter
                {
                    AdoptionRegistrationId = adoptionForm.AdoptionRegistrationId,
                    PetProfile = _petProfileRepo.GetPetProfileById2(adoptionForm.PetProfileId),
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
                    InsertedAt = adoptionForm.InsertedAt.AddHours(ConstHelper.UTC_VIETNAM),
                    UpdatedBy = adoptionForm.UpdatedBy,
                    UpdatedAt = adoptionForm.UpdatedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    Dob  = adoptionForm.Dob,
                });
            }
            return result;
        }
        private bool IsExistedForm(Guid insertedBy, Guid petProfileId)
        {
            var result = _adotionRegistrationFormRepo.Get().FirstOrDefault(s => s.InsertedBy.Equals(insertedBy) && s.PetProfileId.Equals(petProfileId) && s.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.PROCESSING);
            if(result != null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> CancelAdoptionRegistrationForm(UpdateViewModel model, Guid updatedBy,List<string> roleName, string path)
        {
            var form = _adotionRegistrationFormRepo.UpdateAdoptionRegistrationFormStatus(model, updatedBy);
            if (form.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.CANCEL)
            {
                if (roleName != null)
                {
                    if (roleName.Contains(RoleConstant.MANAGER))
                    {
                        await _uow.GetService<NotificationTokenDomain>().NotificationForUser(path, form.InsertedBy, ApplicationNameHelper.USER_APP, new Message
                        {
                            Notification = new Notification
                            {
                                Title = NotificationTitleHelper.REJECT_ADOPTION_FORM_TITLE,
                                Body = NotificationBodyHelper.REJECT_ADOPTION_FORM_BODY
                            }
                        });
                    }
                }
                _uow.SaveChanges();
                return true;
            }
            return false;

        }
        public async Task<object> RejectAdoptionFormAfterAccepted(UpdateViewModel model, Guid updatedBy, string path)
        {
            var form = _adotionRegistrationFormRepo.UpdateAdoptionRegistrationFormStatus(model, updatedBy);
            if (form.AdoptionRegistrationStatus == AdoptionRegistrationFormStatusConst.REJECTED)
            {
                Message message = new Message
                {
                    Notification = new Notification
                    {
                        Body = NotificationBodyHelper.REJECT_ADOPTION_FORM_BODY,
                        Title = NotificationTitleHelper.REJECT_ADOPTION_FORM_TITLE
                    }
                };
                await _uow.GetService<NotificationTokenDomain>().NotificationForUser(path, form.InsertedBy, ApplicationNameHelper.USER_APP, message);
                _petProfileRepo.UpdatePetProfile(new UpdatePetProfileModel 
                {
                    PetProfileId = form.PetProfileId,
                    PetStatus = PetStatusConst.FINDINGADOPTER
                }, updatedBy);
                var result = new RejectAdoptionViewModel
                {
                    Reason = model.Reason,
                    Reject = new AdoptionFormModel
                    {
                        AdoptionFormId = form.AdoptionRegistrationId,
                        UserId = form.InsertedBy
                    }
                };
                _uow.SaveChanges();
                return result;
            }
            return null;
        }
    }
}
