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
        public VolunteerRegistrationFormViewModel Create(VolunteerRegistrationFormCreateModel model)
        {
            var volunteerRegistrationFormRepo = uow.GetService<IVolunteerRegistrationFormRepository>();
            var result = volunteerRegistrationFormRepo.Create(model);
            uow.saveChanges();
            if(result != null)
            {
                return new VolunteerRegistrationFormViewModel
                {
                    CenterId = result.CenterId,
                    Dob = result.Dob,
                    Email = result.Email,
                    FirstName = result.FirstName,
                    Gender = result.Gender,
                    LastName = result.LastName,
                    Phone = result.Phone,
                };
            }
            return null;
        }
        public string Edit(VolunteerRegistrationFormUpdateModel model, Guid insertBy)
        {
            var volunteerRegistrationFormRepo = uow.GetService<IVolunteerRegistrationFormRepository>();
            var userDomain = uow.GetService<UserDomain>();
            var form = volunteerRegistrationFormRepo.Get().FirstOrDefault(s => s.VolunteerRegistrationFormId.Equals(model.VolunteerRegistrationFormId));
            var formData = volunteerRegistrationFormRepo.Edit(form,model);
            var result = "";
            if(model.Status == VolunteerRegistrationFormConst.APPROVE)
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
                var listForm = volunteerRegistrationFormRepo.Get().Where(s => s.Email.Equals(form.Email)).ToList();
                foreach(var item in listForm)
                {
                    //item.Status = VolunteerRegistrationFormConst.REJECT;
                    volunteerRegistrationFormRepo.Update(item);
                }
                uow.saveChanges();
                MailArguments mailArguments = MailFormat.MailModel(form.Email, MailConstant.ApproveRegistrationCenter(form.Email), MailConstant.APPROVE_REGISTRATION_FORM);
                MailExtensions.SendBySendGrid(mailArguments, null, null);
            }
            else
            {
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
            var listForm = volunteerRegistrationFormRepo.Get().Where(s => s.CenterId.Equals(centerId)).ToList();
            var result = new VolunteerViewModel();
            result.Count = listForm.Count();
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
                    //Status = form.Status
                });
            }
            return result;
        }
    }
}
