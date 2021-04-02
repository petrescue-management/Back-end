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
    public class VolunteerRegistrationFormDomain : BaseDomain
    {
        public VolunteerRegistrationFormDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public string Create(VolunteerRegistrationFormCreateModel model)
        {
            var volunteerRegistrationFormRepo = uow.GetService<IVolunteerRegistrationFormRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var currentUser = userRepo.Get().FirstOrDefault(s => s.UserEmail.Equals(model.Email));
            var result = "";
            if(currentUser == null)
            {
                volunteerRegistrationFormRepo.Create(model);
                uow.saveChanges();
                result = "Success";
            }
            else
            {
                if ((bool)!currentUser.IsBelongToCenter)
                {
                    volunteerRegistrationFormRepo.Create(model);
                    uow.saveChanges();
                    result = "Success";
                }
                result = "This email is belong anoter center";
            }
            return result;
        }
        public string Edit(VolunteerRegistrationFormUpdateModel model, Guid insertBy)
        {
            var volunteerRegistrationFormRepo = uow.GetService<IVolunteerRegistrationFormRepository>();
            var userDomain = uow.GetService<UserDomain>();
            var centerRepo = uow.GetService<ICenterRepository>();
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
                    var listForm = volunteerRegistrationFormRepo.Get().Where(s => s.Email.Equals(form.Email) && s.Status == VolunteerRegistrationFormConst.PROCESSING).ToList();
                    foreach (var item in listForm)
                    {
                        item.Status = VolunteerRegistrationFormConst.REJECT;
                        volunteerRegistrationFormRepo.Update(item);
                    }
                    var center = centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(form.CenterId));
                    var centerModel = new CenterViewModel
                    {
                        Address = center.Address,
                        CenterName = center.CenterName,
                        Phone = center.Phone,
                    };
                    MailArguments mailArguments = MailFormat.MailModel(form.Email, MailConstant.ApproveRegistrationVolunteer(form.Email,centerModel), MailConstant.APPROVE_REGISTRATION_VOLUNTEER);
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
                };
                var reason = "";
                if (model.IsEmail)
                    reason += ErrorConst.ErrorEmail;
                if (model.IsPhone)
                    reason += ErrorConst.ErrorPhone;
                if (model.AnotherReason != null)
                    reason += model.AnotherReason + "<br/>";
                result = "reject";
                uow.saveChanges();
                MailArguments mailArguments = MailFormat.MailModel(form.Email, MailConstant.RejectRegistrationCenter(form.Email), MailConstant.REJECT_REGISTRATION_FORM);
                MailExtensions.SendBySendGrid(mailArguments, null, null);
            }
            return result;
        }
        public VolunteerViewModel GetListVolunteerRegistrationForm(Guid centerId)
        {
            var volunteerRegistrationFormRepo = uow.GetService<IVolunteerRegistrationFormRepository>();
            var listForm = volunteerRegistrationFormRepo.Get().Where(s => s.CenterId.Equals(centerId) && s.Status == VolunteerRegistrationFormConst.PROCESSING).ToList();
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
                    Status = form.Status,
                    FormId = form.VolunteerRegistrationFormId,
                    InsertAt = form.InsertAt
                });
            }
            return result;
        }
    }
}
