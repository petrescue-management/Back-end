using Microsoft.AspNetCore.Hosting;
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

namespace PetRescue.Data.Domains
{
    public class CenterRegistrationFormDomain : BaseDomain
    {
        private readonly ICenterRegistrationFormRepository _centerRegistrationRepo;
        private readonly ICenterRepository _centerRepo;
        private readonly IUserRepository _userRepo;

        public CenterRegistrationFormDomain(IUnitOfWork uow, 
            ICenterRegistrationFormRepository centerRegistrationFormRepo, 
            ICenterRepository centerRepo, 
            IUserRepository userRepo) : base(uow)
        {
            this._centerRegistrationRepo = centerRegistrationFormRepo;
            this._centerRepo = centerRepo;
            this._userRepo = userRepo;
        }

        #region SEARCH
        public SearchReturnModel SearchCenterRegistrationForm(SearchModel model)
        {
            var records = _centerRegistrationRepo.Get().AsQueryable();
            if (model.Status != 0)
                records = records.Where(f => f.CenterRegistrationFormStatus.Equals(model.Status));

            List<CenterRegistrationFormModel> result = records
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(f => new CenterRegistrationFormModel
                {
                    CenterRegistrationFormId = f.CenterRegistrationFormId,
                    CenterName = f.CenterName,
                    Email = f.Email,
                    Phone = f.Phone,
                    Lat = f.Lat,
                    Lng = f.Lng,
                    CenterAddress = f.CenterAddress,
                    Description = f.Description,
                    CenterRegistrationFormStatus = f.CenterRegistrationFormStatus,
                    UpdatedAt = f.UpdatedAt,
                    ImageUrl = f.CenterImgUrl,
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
            var form = _centerRegistrationRepo.GetCenterRegistrationFormById(id);
            return form;
        }
        #endregion

        #region CREATE
        public string CreateCenterRegistrationForm(CreateCenterRegistrationFormModel model)
        {
            //check Duplicate  phone
            var check_dup_phone = _centerRepo.Get()
                .Where(c => c.Phone.Equals(model.Phone));

            //check Duplicate email
            var check_dup_email = _userRepo.Get()
               .Where(u => u.UserEmail.Contains(model.Email));

            //check Duplicate address
            var check_dup_address = _centerRepo.Get()
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

            var form = _centerRegistrationRepo.CreateCenterRegistrationForm(model);
            _uow.SaveChanges();
            return form.CenterRegistrationFormId.ToString();
        }
        #endregion

        #region PROCESS FORM
        public string ProcressCenterRegistrationForm(UpdateRegistrationCenter model, Guid insertBy)
        {
            var form = _centerRegistrationRepo.Get().FirstOrDefault(s => s.CenterRegistrationFormId.Equals(model.Id));
            var _userRoleDomain = _uow.GetService<UserRoleDomain>();
            var result = "";
            //Find user
            var currentUser = _userRepo.FindById(form.Email);
            if (form != null)
            {
                //Status == Approved
                if (model.Status == CenterRegistrationFormStatusConst.APPROVED)
                {
                    var context = _uow.GetService<DbContext>();
                    // Make a transaction
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            //update Status
                            form = _centerRegistrationRepo.UpdateCenterRegistrationStatus(form, model, insertBy);
                            //create Center
                            var newCenter = _centerRepo.CreateCenter(new CreateCenterModel
                            {
                                Address = form.CenterAddress,
                                CenterName = form.CenterName,
                                Phone = form.Phone,
                                Lng = form.Lng,
                                Lat = form.Lat,
                                ImageUrl = form.CenterImgUrl,
                                CenterId = form.CenterRegistrationFormId
                            }, insertBy);
                            if (currentUser == null) //not found user
                            {
                                //Create Model for create new User
                                var newCreateUserModel = new UserCreateModel
                                {
                                    Email = form.Email,
                                    IsBelongToCenter = UserConst.BELONG,
                                    CenterId = newCenter.CenterId
                                };
                                // create new Role for newUser
                                var newUser = _userRepo.CreateUserByModel(newCreateUserModel);
                                var newUserRoleUpdateModel = new UserRoleUpdateModel
                                {
                                    RoleName = RoleConstant.MANAGER,
                                    UserId = newUser.UserId,
                                };
                                _uow.GetService<UserDomain>().AddRoleManagerToUser(newUserRoleUpdateModel, insertBy);
                                transaction.Commit();
                                result = newCenter.CenterId.ToString();
                            }
                            else // found user
                            {
                                if (currentUser.CenterId == null)
                                {
                                    //Create Model for update user
                                    var userUpdateModel = new UserUpdateModel
                                    {
                                        CenterId = newCenter.CenterId
                                        //CenterId = newCenter.CenterId
                                    };
                                    currentUser = _userRepo.UpdateUserModel(currentUser, userUpdateModel);
                                    //Create new Role for currentUser
                                    var userRole = _userRoleDomain.CheckRoleOfUser(new UserRoleUpdateModel 
                                    {
                                        RoleName = RoleConstant.MANAGER,
                                        UserId = currentUser.UserId
                                    });
                                    if (userRole != null)
                                    {
                                        userRole = _userRoleDomain.Edit(userRole, new UserRoleUpdateEntityModel 
                                        {
                                            IsActived = true,
                                            UpdateBy = insertBy
                                        });
                                    }
                                    else
                                    {
                                        var userRoleUpdateModel = new UserRoleUpdateModel
                                        {
                                            RoleName = RoleConstant.MANAGER,
                                            UserId = currentUser.UserId,
                                        };
                                        _userRoleDomain.RegistationRole(currentUser.UserId, RoleConstant.MANAGER, insertBy);
                                    }
                                    
                                    
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
                            _uow.SaveChanges();
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
                    form = _centerRegistrationRepo.UpdateCenterRegistrationStatus(form, model, insertBy);
                    _uow.SaveChanges();
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
                    result = form.CenterRegistrationFormId.ToString();
                }
            }
            return result;
        }
        #endregion
    }
}
