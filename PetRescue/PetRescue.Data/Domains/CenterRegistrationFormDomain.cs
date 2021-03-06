﻿using PetRescue.Data.ConstantHelper;
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
    public class CenterRegistrationFormDomain : BaseDomain
    {
        public CenterRegistrationFormDomain(IUnitOfWork uow) : base(uow)
        {
        }

        #region SEARCH
        public SearchReturnModel SearchCenterRegistrationForm(SearchModel model)
        {
            var records = uow.GetService<ICenterRegistrationFormRepository>().Get()
                .Where(f => f.CenterRegistrationStatus == CenterRegistrationFormStatusConst.PROCESSING);

            List<CenterRegistrationFormModel> result = records
                .Skip((model.PageIndex - 1) * 10)
                .Take(10)
                .Select(f => new CenterRegistrationFormModel
                {
                    CenterRegistrationId = f.CenterRegistrationId,
                    CenterName = f.CenterName,
                    Email = f.Email,
                    Phone = f.Phone,
                    CenterAddress = f.CenterAddress,
                    Description = f.Description,
                    CenterRegistrationStatus = f.CenterRegistrationStatus,
                    UpdatedAt = f.UpdatedAt
                }).ToList();
            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }
        #endregion

        #region GET BY ID
        public CenterRegistrationFormModel GetCenterRegistrationFormById(Guid id)
        {
            var form = uow.GetService<ICenterRegistrationFormRepository>().GetCenterRegistrationFormById(id);
            return form;
        }
        #endregion

        #region CREATE
        public string CreateCenterRegistrationForm(CreateCenterRegistrationFormModel model)
        {
            //call UserService
            var user_service = uow.GetService<IUserRepository>();

            //call CenterService
            var center_service = uow.GetService<ICenterRepository>();

            //check Duplicate  phone
            var check_dup_phone = center_service.Get()
                .Where(c => c.Phone.Equals(model.Phone));

            //check Duplicate email
            var check_dup_email = user_service.Get()
               .Where(u => u.UserEmail.Equals(model.Email));

            //check Duplicate address
            var check_dup_address = center_service.Get()
               .Where(c => c.Address.Equals(model.CenterAddress));

            //dup phone & email
            if (check_dup_phone.Any() && check_dup_email.Any())
                return "This phone and email  is already registered !";

            //dup phone & address
            if (check_dup_phone.Any() && check_dup_address.Any())
                return "This phone and address  is already registered !";

            //dup email & address
            if (check_dup_email.Any() && check_dup_address.Any())
                return "This email and address  is already registered !";

            //dup phone
            if (check_dup_phone.Any())
                return "This phone is already registered !";

            //dup email
            if (check_dup_email.Any())
                return "This email is already registered !";

            //dup address
            if (check_dup_address.Any())
                return "This address is already registered !";

            var form = uow.GetService<ICenterRegistrationFormRepository>().CreateCenterRegistrationForm(model);
            return form.CenterRegistrationId.ToString();
        }
        #endregion

        #region PROCESS FORM
        public CenterRegistrationFormModel ProcressCenterRegistrationForm(UpdateStatusModel model)
        {
            var center_registration_form_service = uow.GetService<ICenterRegistrationFormRepository>();
            var center_service = uow.GetService<ICenterRepository>();
            var userDomain = uow.GetService<UserDomain>();
            var userRepo = uow.GetService<IUserRepository>();
            var form = center_registration_form_service.GetCenterRegistrationFormById(model.Id);
            if(form != null)
            {
                //Status == Approved
                if (model.Status == CenterRegistrationFormStatusConst.APPROVED)
                {
                    var context = uow.GetService<PetRescueContext>();
                    using (var transaction = context.Database.BeginTransaction())
                    {             
                        try
                        {
                            //update Status
                            form = center_registration_form_service.UpdateCenterRegistrationStatus(model);

                            //create Center
                            var newCenter = center_service.CreateCenter(new CreateCenterModel
                            {
                                Address = form.CenterAddress,
                                CenterName = form.CenterName,
                                Phone = form.Phone,
                            });

                            //Create Model for create new User
                            var newCreateUserModel = new UserCreateModel
                            {
                                Email = form.Email,
                                CenterId = newCenter.CenterId,
                                isBelongToCenter = UserConst.BELONG,
                            };
                            // create new Account
                            var newUser = userRepo.CreateUserByModel(newCreateUserModel);


                            //Create Model for add Role to User
                            var newUserRoleUpdateModel = new UserRoleUpdateModel
                            {
                                CenterId = newCenter.CenterId,
                                RoleName = RoleConstant.Manager,
                                UserId = newUser.UserId,
                            };
                            userDomain.AddRoleManagerToUser(newUserRoleUpdateModel);
                            transaction.Commit();
                            return form;
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            throw (e);
                        }

                    }
                  
                }

                //Status = Rejected
                else if (model.Status == CenterRegistrationFormStatusConst.REJECTED)
                {
                    form = center_registration_form_service.UpdateCenterRegistrationStatus(model);
                    return form;
                }
            }
            return null;
        }
        #endregion
    }
}
