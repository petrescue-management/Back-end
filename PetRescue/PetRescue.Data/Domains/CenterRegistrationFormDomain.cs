﻿using Microsoft.AspNetCore.Hosting;
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
    public class CenterRegistrationFormDomain : BaseDomain
    {
        public CenterRegistrationFormDomain(IUnitOfWork uow) : base(uow)
        {
        }

        #region SEARCH
        public SearchReturnModel SearchCenterRegistrationForm(SearchModel model)
        {
            var records = uow.GetService<ICenterRegistrationFormRepository>().Get().AsQueryable();
            if (model.Status != 0)
                records = records.Where(f => f.CenterRegistrationStatus.Equals(model.Status));

            List<CenterRegistrationFormModel> result = records
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(f => new CenterRegistrationFormModel
                {
                    CenterRegistrationId = f.CenterRegistrationId,
                    CenterName = f.CenterName,
                    Email = f.Email,
                    Phone = f.Phone,
                    Lat = f.Lat,
                    Lng = f.Lng,
                    CenterAddress = f.CenterAddress,
                    Description = f.Description,
                    CenterRegistrationStatus = f.CenterRegistrationStatus,
                    UpdatedAt = f.UpdatedAt,
                    ImageUrl = f.ImageUrl,
                    InsertedAt = f.InsertedAt
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
               .Where(u => u.IsBelongToCenter == true && u.UserEmail.Contains(model.Email));

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
            uow.saveChanges();
            return form.CenterRegistrationId.ToString();
        }
        #endregion

        #region PROCESS FORM
        public string ProcressCenterRegistrationForm(UpdateRegistrationCenter model, Guid insertBy)
        {
            var center_registration_form_service = uow.GetService<ICenterRegistrationFormRepository>();
            var center_service = uow.GetService<ICenterRepository>();
            var userDomain = uow.GetService<UserDomain>();
            var userRepo = uow.GetService<IUserRepository>();
            var workingHistoryRepo = uow.GetService<IWorkingHistoryRepository>();
            var form = center_registration_form_service.GetCenterRegistrationFormById(model.Id);
            var result = "";
            //Find user
            var currentUser = userRepo.FindById(form.Email);
            if (form != null)
            {
                //Status == Approved
                if (model.Status == CenterRegistrationFormStatusConst.APPROVED)
                {
                    var context = uow.GetService<PetRescueContext>();
                    // Make a transaction
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //update Status
                            form = center_registration_form_service.UpdateCenterRegistrationStatus(model, insertBy);
                            //create Center
                            var newCenter = center_service.CreateCenter(new CreateCenterModel
                            {
                                Address = form.CenterAddress,
                                CenterName = form.CenterName,
                                Phone = form.Phone,
                                Lng = form.Lng,
                                Lat = form.Lat,
                                ImageUrl = form.ImageUrl
                            }, insertBy);
                            if (currentUser == null) //not found user
                            {
                                //Create Model for create new User
                                var newCreateUserModel = new UserCreateModel
                                {
                                    Email = form.Email,
                                    IsBelongToCenter = UserConst.BELONG,
                                };
                                // create new Role for newUser
                                var newUser = userRepo.CreateUserByModel(newCreateUserModel);
                                var newUserRoleUpdateModel = new UserRoleUpdateModel
                                {
                                    CenterId = newCenter.CenterId,
                                    RoleName = RoleConstant.MANAGER,
                                    UserId = newUser.UserId,
                                };
                                userDomain.AddRoleManagerToUser(newUserRoleUpdateModel, insertBy);
                                workingHistoryRepo.Create(new WorkingHistoryCreateModel
                                {
                                    CenterId = newCenter.CenterId,
                                    Description = "",
                                    RoleName = RoleConstant.MANAGER,
                                    UserId = newUser.UserId
                                });
                                transaction.Commit();
                                result = newCenter.CenterId.ToString();
                            }
                            else // found user
                            {
                                if ((bool)!currentUser.IsBelongToCenter)
                                {
                                    //Create Model for update user
                                    var userUpdateModel = new UserUpdateModel
                                    {
                                        IsBelongToCenter = UserConst.BELONG,
                                        //CenterId = newCenter.CenterId
                                    };
                                    currentUser = userRepo.UpdateUserModel(currentUser, userUpdateModel);
                                    //Create new Role for currentUser
                                    var userRoleUpdateModel = new UserRoleUpdateModel
                                    {
                                        CenterId = newCenter.CenterId,
                                        RoleName = RoleConstant.MANAGER,
                                        UserId = currentUser.UserId,
                                    };
                                    userDomain.AddRoleManagerToUser(userRoleUpdateModel, insertBy);
                                    workingHistoryRepo.Create(new WorkingHistoryCreateModel
                                    {
                                        CenterId = newCenter.CenterId,
                                        Description = "",
                                        RoleName = RoleConstant.MANAGER,
                                        UserId = currentUser.UserId
                                    });
                                    transaction.Commit();
                                }
                                else
                                {
                                    result = "this email is belong another center";
                                    return result;
                                }
                            }
                            var viewModel = new CenterRegistrationFormViewModel
                            {
                                CenterName = form.CenterName,
                                Email = form.Email
                            };
                            uow.saveChanges();
                            MailArguments mailArguments = MailFormat.MailModel(form.Email, MailConstant.ApproveRegistrationCenter(viewModel), MailConstant.APPROVE_REGISTRATION_FORM);
                            MailExtensions.SendBySendGrid(mailArguments, null, null);
                            result = newCenter.CenterId.ToString();
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
                    form = center_registration_form_service.UpdateCenterRegistrationStatus(model, insertBy);
                    uow.saveChanges();
                    var error = "";
                    if (model.IsAddress)
                        error += ErrorConst.ErrorAddress;
                    if (model.IsImage)
                        error += ErrorConst.ErrorImage;
                    if (model.IsMail)
                        error += ErrorConst.ErrorEmail;
                    if (model.IsPhone)
                        error += ErrorConst.ErrorPhone;
                    if (model.AnotherReason != null)
                        error += "<li><p>" + model.AnotherReason + "</p></li>";
                    var viewModel = new CenterRegistrationFormViewModel
                    {
                        CenterName = form.CenterName,
                        Email = form.Email
                    };
                    MailArguments mailArguments = MailFormat.MailModel(form.Email, MailConstant.RejectRegistrationCenter(viewModel, error), MailConstant.REJECT_REGISTRATION_FORM);
                    MailExtensions.SendBySendGrid(mailArguments, null, null);
                    result = form.CenterRegistrationId.ToString();
                }
            }
            return result;
        }
        #endregion
    }
}
