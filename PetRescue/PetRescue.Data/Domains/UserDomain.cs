﻿using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Extensions;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace PetRescue.Data.Domains
{
    public class UserDomain : BaseDomain
    {
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IUserRepository _userRepo;
        private readonly ICenterRepository _centerRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly INotificationTokenRepository _notificationTokenRepo;
        private readonly DbContext _context;
        public UserDomain(IUnitOfWork uow, 
            IUserProfileRepository userProfileRepo, 
            IUserRepository userRepo, 
            ICenterRepository centerRepo, 
            IRoleRepository roleRepo, 
            IUserRoleRepository userRoleRepo, 
            INotificationTokenRepository notificationTokenRepo, 
            DbContext context) : base(uow)
        {
            this._userProfileRepo = userProfileRepo;
            this._userRepo = userRepo;
            this._centerRepo = centerRepo;
            this._roleRepo = roleRepo;
            this._userRoleRepo = userRoleRepo;
            this._notificationTokenRepo = notificationTokenRepo;
            this._context = context;
        }
        public object GetUserDetail(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var result = handler.ReadJwtToken(token) as JwtSecurityToken;
            var currentClaims = result.Claims.ToList();
            string email = currentClaims.FirstOrDefault(t => t.Type == "email").Value;
            var user = _userRepo.Get().FirstOrDefault(u => u.UserEmail == email);
            if (user == null)
            {
                return new
                {
                    message = "Not Found User"
                };
            }
            var userProfile = _userProfileRepo.FindById(user.UserId);
            if (userProfile != null)
            {
                var returnResult = new UserDetailModel
                {
                    Email = user.UserEmail,
                    Id = user.UserId.ToString(),
                    Roles = user.UserRole.Where(r => (bool)r.IsActived).Select(r => r.Role.RoleName).ToArray(),
                    CenterId = user.CenterId,
                    Phone = userProfile.Phone,
                    DoB = userProfile.Dob,
                    FirstName = userProfile.FirstName,
                    Gender = userProfile.Gender,
                    LastName = userProfile.LastName,
                    ImgUrl = userProfile.UserImgUrl,
                    UpdatedAt = userProfile.UpdatedAt,
                };
                var center = new CenterProfileViewModel();
                if (user.CenterId != null)
                {
                    var centerProfile = _centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(user.CenterId));
                    center.CenterAddrress = centerProfile.Address;
                    center.CenterName = centerProfile.CenterName;
                }
                returnResult.Center = center;
                return returnResult;
            }
            else
            {
                var returnResult = new UserDetailModel
                {
                    Email = user.UserEmail,
                    Id = user.UserId.ToString(),
                    Roles = user.UserRole.Select(r => r.Role.RoleName).ToArray(),
                    CenterId = user.CenterId,
                };
                return returnResult;
            }

        }
        public bool UpdateUserProfile(UserProfileUpdateModel model)
        {
            var userProfile = _userProfileRepo.FindById(model.UserId);
            var result = userProfile == null ? _userProfileRepo.Create(model)
                : _userProfileRepo.Edit(userProfile, model);
            if (result != null)
            {
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
        public string AddRoleManagerToUser(UserRoleUpdateModel model, Guid insertBy)
        {
            var newRole = _uow.GetService<UserRoleDomain>().RegistationRole(model.UserId, model.RoleName, insertBy);
            if (newRole != null)
            {
                return model.UserId.ToString();
            }
            return null;
        }
        public List<NotificationToken> GetListDeviceTokenByRoleAndApplication(string roleName, string applicationName)
        {
            var roleId = _roleRepo.Get().FirstOrDefault(r => r.RoleName.Equals(roleName)).RoleId;
            var listUserRole = _userRoleRepo.Get().Where(s => s.RoleId.Equals(roleId)).ToList();
            var listNotificationToken = new List<NotificationToken>();
            foreach (var userRole in listUserRole)
            {
                var notificationToken = _notificationTokenRepo.Get().FirstOrDefault(s => s.UserId.Equals(userRole.UserId) && s.ApplicationName.Equals(applicationName));
                if (notificationToken != null)
                {
                    listNotificationToken.Add(notificationToken);
                }
            }
            return listNotificationToken;
        }
        public NotificationToken GetManagerDeviceTokenByCenterId(Guid centerId)
        {
            var currentUser = _userRepo.Get().FirstOrDefault(s => s.CenterId.Equals(centerId));
            var notificationToken = _notificationTokenRepo.Get().FirstOrDefault(s => s.UserId.Equals(currentUser.UserId) && s.ApplicationName.Equals(ApplicationNameHelper.MANAGE_CENTER_APP));
            return notificationToken;
        }
        public string AddVolunteerRole(AddNewRoleModel model)
        {
            var currentUser = _userRepo.Get().FirstOrDefault(s => s.UserEmail.Equals(model.Email));
            var _userRoleDomain = _uow.GetService<UserRoleDomain>();
            var result = "This is not found";
            if (currentUser != null)
            {
                //find role of User
                var userRole = _userRoleDomain.CheckRoleOfUser(new UserRoleUpdateModel
                {
                    RoleName = model.RoleName,
                    UserId = currentUser.UserId
                });
                // if user isn't exist
                if (userRole == null)
                {
                    _userRoleDomain.RegistationRole(currentUser.UserId, model.RoleName, model.InsertBy);
                    _userRepo.UpdateUserStatus(currentUser, UserStatus.OFFLINE);
                    result = currentUser.UserId.ToString();
                }
                else
                {
                    if (!(bool)userRole.IsActived)
                    {
                        _userRoleDomain.Edit(userRole, new UserRoleUpdateEntityModel
                        {
                            IsActived = true,
                            UpdateBy = model.InsertBy
                        });
                        result = currentUser.UserId.ToString();
                    }
                }
            }
            else
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var newUser = _userRepo.CreateUser(new UserCreateByAppModel 
                        {
                            Email = model.Email,
                            Status = UserStatus.OFFLINE
                        });
                        _userRoleDomain.RegistationRole(newUser.UserId, model.RoleName, model.InsertBy);
                        UpdateUserProfile(new UserProfileUpdateModel
                        {
                            DoB = model.DoB,
                            FirstName = model.FirstName,
                            Gender = model.Gender,
                            LastName = model.LastName,
                            Phone = model.Phone,
                            UserId = newUser.UserId,
                        });
                        transaction.Commit();
                        result = newUser.UserId.ToString();
                    }
                    catch
                    {
                        transaction.Rollback();
                        result = "This cannot create";
                    }
                }
            }
            return result;
        }
        public string[] GetRoleOfUser(Guid userId)
        {
            var user = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(userId));
            if (user != null)
            {
                return user.UserRole.Where(s => (bool)s.IsActived).Select(r => r.Role.RoleName).ToArray();
            }
            return null;
        }
        public string RemoveVolunteerOfCenter(RemoveVolunteerRoleModel model)
        {
            var _userRoleDomain = _uow.GetService<UserRoleDomain>();
            var _notificationTokenDomain = _uow.GetService<NotificationTokenDomain>();
            var userRole = _userRoleDomain.CheckRoleOfUser(new UserRoleUpdateModel
            {
                RoleName = RoleConstant.VOLUNTEER,
                UserId = model.UserId
            });
            var result = "Not Found";
            if (userRole != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(model.UserId));
                        _userRoleDomain.Edit(userRole, new UserRoleUpdateEntityModel
                        {
                            IsActived = false,
                            UpdateBy = model.InsertBy
                        });
                        _notificationTokenDomain.DeleteNotificationByUserIdAndApplicationName(currentUser.UserId, ApplicationNameHelper.VOLUNTEER_APP);
                        _userRepo.UpdateUserStatus(currentUser, null);
                        _uow.SaveChanges();
                        transaction.Commit();
                        result = "";
                    }
                    catch
                    {
                        transaction.Rollback();
                        result = "Error";
                    }
                }
            }
            return result;
        }
        public object GetListProfileOfVolunter()
        {
            var result = new List<UserProfileViewModel2>();
            var role = _roleRepo.Get().FirstOrDefault(s => s.RoleName.Equals(RoleConstant.VOLUNTEER));
            var userRoles = _userRoleRepo.Get().Where(s => s.RoleId.Equals(role.RoleId) && (bool)s.IsActived);
            foreach(var userRole in userRoles)
            {
                result.Add(new UserProfileViewModel2 
                {
                    DateStarted = userRole.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    DoB = userRole.User.UserProfile.Dob,
                    Email = userRole.User.UserEmail,
                    FirstName = userRole.User.UserProfile.FirstName,
                    LastName = userRole.User.UserProfile.LastName,
                    Gender = userRole.User.UserProfile.Gender,
                    ImgUrl = userRole.User.UserProfile.UserImgUrl,
                    Phone = userRole.User.UserProfile.Phone,
                    UserId = userRole.UserId
                });
            }
            return result;
        }
        public object GetListProfileMember(int page, int limit)
        {
            var users = _userRepo.Get().Where(s => s.UserProfile != null);
            if(users != null)
            {
                var total = 0;
                if (limit == 0)
                {
                    limit = 1;
                }
                if (limit > -1)
                {
                    total = users.Count() / limit;
                }
                users = users.OrderBy(s => s.UserProfile.FirstName);
                if (limit > -1 && page >= 0)
                {
                    users = users.Skip(page * limit).Take(limit);
                }
                var listUsers = new List<UserProfileViewModel>();
                foreach(var user in users)
                {
                    listUsers.Add(new UserProfileViewModel 
                    {
                        Email = user.UserEmail,
                        DoB = user.UserProfile.Dob,
                        FirstName = user.UserProfile.FirstName,
                        Gender = user.UserProfile.Gender,
                        ImgUrl = user.UserProfile.UserImgUrl,
                        LastName = user.UserProfile.LastName,
                        Phone = user.UserProfile.Phone,
                        UserId = user.UserId
                    });
                }
                var result = new Dictionary<string, object>()
                {
                    ["totalPages"] = total,
                    ["result"] = listUsers
                };
                return result;
            }
            return null;
        }
        public UserProfileViewModel GetProfileByUserId(Guid userId)
        {
            
            var user = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(userId));
            if(user != null)
            {
                return new UserProfileViewModel
                {
                    Email = user.UserEmail,
                    DoB = user.UserProfile.Dob,
                    FirstName = user.UserProfile.FirstName,
                    Gender = user.UserProfile.Gender,
                    ImgUrl = user.UserProfile.UserImgUrl,
                    LastName = user.UserProfile.LastName,
                    Phone = user.UserProfile.Phone,
                    UserId = user.UserId
                };
            }
            return null;
        }
        public Guid GetUserIdByEmail(string email)
        {
            var currentUser = _userRepo.Get().FirstOrDefault(s => email.Equals(s.UserEmail));
            if(currentUser != null)
            {
                return currentUser.UserId;
            }
            return Guid.Empty;
        }
    }
}
