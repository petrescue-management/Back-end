using FirebaseAdmin.Messaging;
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
    public class VolunteerRegistrationFormDomain : BaseDomain
    {
        public VolunteerRegistrationFormDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public async Task<string> Create(VolunteerRegistrationFormCreateModel model, string path)
        {
            var volunteerRegistrationFormRepo = uow.GetService<IVolunteerRegistrationFormRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var currentUser = userRepo.Get().FirstOrDefault(s => s.UserEmail.Equals(model.Email));
            var userRoleDomain = uow.GetService<UserRoleDomain>();
            var notificationTokenDomain = uow.GetService<NotificationTokenDomain>();
            var result = "";
            if (!IsExisted(model.Email, model.CenterId))
            {
                Message message = new Message
                {
                    Notification = new Notification
                    {
                        Title = NotificationTitleHelper.NEW_VOLUNTEER_FORM_TITLE,
                        Body = NotificationBodyHelper.NEW_VOLUNTEER_FORM_BODY
                    }
                };
                if (currentUser == null)
                {
                    var form =volunteerRegistrationFormRepo.Create(model);
                    await notificationTokenDomain.NotificationForManager(path,model.CenterId, message);
                    uow.saveChanges();
                    result = form.VolunteerRegistrationFormId.ToString();
                }
                else
                {
                    if ((bool)!currentUser.IsBelongToCenter)
                    {
                        if (!userRoleDomain.IsAdmin(model.Email))
                        {
                            var form = volunteerRegistrationFormRepo.Create(model);
                            await notificationTokenDomain.NotificationForManager(path, model.CenterId, message);
                            uow.saveChanges();
                            result = form.VolunteerRegistrationFormId.ToString();
                        }
                        else
                        {
                            result = "This email is invalid";
                        }
                    }
                    else
                    {
                        result = "This email is belong anoter center";
                    }
                }
            }
            else
            {
                result = "This Volunteer Form is Existed";
            }
            return result;
        }
        public string Edit(VolunteerRegistrationFormUpdateModel model, Guid insertBy)
        {
            var volunteerRegistrationFormRepo = uow.GetService<IVolunteerRegistrationFormRepository>();
            var userDomain = uow.GetService<UserDomain>();
            var centerRepo = uow.GetService<ICenterRepository>();
            var workingHistoryRepo = uow.GetService<IWorkingHistoryRepository>();
            var form = volunteerRegistrationFormRepo.Get().FirstOrDefault(s => s.VolunteerRegistrationFormId.Equals(model.VolunteerRegistrationFormId));
            var formData = volunteerRegistrationFormRepo.Edit(form, model);
            var result = "";
            if (model.Status == VolunteerRegistrationFormConst.APPROVE)
            {
                var newModel = new AddNewRoleModel
                {
                    CenterId = formData.CenterId,
                    DoB = formData.Dob,
                    Email = formData.Email,
                    FirstName = formData.FirstName,
                    Gender = Byte.Parse(formData.Gender.ToString()),
                    InsertBy = insertBy,
                    LastName = formData.LastName,
                    Phone = formData.Phone,
                    RoleName = RoleConstant.VOLUNTEER,
                };
                result = userDomain.AddUserToCenter(newModel);
                if (!result.Contains("This"))
                {
                    userDomain.UpdateUserProfile(new UserProfileUpdateModel
                    {
                        DoB = form.Dob,
                        FirstName = form.FirstName,
                        Gender = (byte)form.Gender,
                        ImgUrl = form.VolunteerRegistrationFormImageUrl,
                        LastName = form.LastName,
                        Phone = form.Phone,
                        UserId = userDomain.GetUserIdByEmail(form.Email)
                    });
                    var listForm = volunteerRegistrationFormRepo.Get().Where(s => s.Email.Equals(form.Email) && s.VolunteerRegistrationFormStatus == VolunteerRegistrationFormConst.PROCESSING).ToList();
                    foreach (var item in listForm)
                    {
                        item.VolunteerRegistrationFormStatus = VolunteerRegistrationFormConst.REJECT;
                        volunteerRegistrationFormRepo.Update(item);
                    }
                    workingHistoryRepo.Create(new WorkingHistoryCreateModel
                    {
                        CenterId = formData.CenterId,
                        Description = "",
                        RoleName = RoleConstant.VOLUNTEER,
                        UserId = userDomain.GetUserIdByEmail(form.Email)
                    });
                    var center = centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(form.CenterId));
                    var centerModel = new CenterViewModel
                    {
                        Address = center.Address,
                        CenterName = center.CenterName,
                        Phone = center.Phone,
                        Email = center.CenterNavigation.Email,
                    };
                    var volunteerFormModel = new VolunteerRegistrationFormViewModel
                    {
                        FirstName = formData.FirstName,
                        LastName = formData.LastName,
                    };
                    MailArguments mailArguments = MailFormat.MailModel(form.Email, MailConstant.ApproveRegistrationVolunteer(volunteerFormModel, centerModel), MailConstant.APPROVE_REGISTRATION_VOLUNTEER);
                    MailExtensions.SendBySendGrid(mailArguments, null, null);
                    uow.saveChanges();
                }
                return result;
            }
            else
            {
                var center = centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(form.CenterId));
                var centerModel = new CenterViewModel
                {
                    Address = center.Address,
                    CenterName = center.CenterName,
                    Phone = center.Phone,
                    Email = center.CenterNavigation.Email,
                };
                var reason = "";
                if (model.IsEmail)
                    reason += ErrorConst.ErrorEmail;
                if (model.IsPhone)
                    reason += ErrorConst.ErrorPhone;
                if (model.IsName)
                    reason += ErrorConst.ErrorName;
                if (model.AnotherReason != null)
                    reason += "<li><p>"+model.AnotherReason + "</p></li>";
                result = "reject";
                var volunteerFormModel = new VolunteerRegistrationFormViewModel
                {
                    FirstName = formData.FirstName,
                    LastName = formData.LastName,
                };
                uow.saveChanges();
                MailArguments mailArguments = MailFormat.MailModel(form.Email, MailConstant.RejectRegistrationVolunteer(volunteerFormModel, reason, centerModel), MailConstant.REJECT_REGISTRATION_FORM);
                MailExtensions.SendBySendGrid(mailArguments, null, null);
            }
            return result;
        }
        public VolunteerViewModel GetListVolunteerRegistrationForm(Guid centerId)
        {
            var volunteerRegistrationFormRepo = uow.GetService<IVolunteerRegistrationFormRepository>();
            var listForm = volunteerRegistrationFormRepo.Get().Where(s => s.CenterId.Equals(centerId) && s.VolunteerRegistrationFormStatus == VolunteerRegistrationFormConst.PROCESSING).ToList();
            var result = new VolunteerViewModel();
            result.Count = listForm.Count();
            result.List = new List<VolunteerRegistrationFormViewModel>();
            foreach(var form in listForm)
            {
                result.List.Add(new VolunteerRegistrationFormViewModel
                {
                    CenterId = form.CenterId,
                    Dob = form.Dob,
                    Email = form.Email,
                    FirstName = form.FirstName,
                    Gender = form.Gender,
                    LastName = form.LastName,
                    Phone = form.Phone,
                    Status = form.VolunteerRegistrationFormStatus,
                    FormId = form.VolunteerRegistrationFormId,
                    InsertAt =form.InsertedAt.AddHours(ConstHelper.UTC_VIETNAM),
                    ImageUrl = form.VolunteerRegistrationFormImageUrl,
                    VolunteerRegistrationFormId = form.VolunteerRegistrationFormId
                });
            }
            return result;
        }
        private bool IsExisted(string email, Guid centerId)
        {
            var volunteerFormRepo = uow.GetService<IVolunteerRegistrationFormRepository>();
            var result = volunteerFormRepo.Get().FirstOrDefault(s => s.CenterId.Equals(centerId) 
                && s.Email.Equals(email) && 
                s.VolunteerRegistrationFormStatus == VolunteerRegistrationFormConst.PROCESSING);
            if(result != null)
            {
                return true;
            }
            return false;
        }
    }
}
