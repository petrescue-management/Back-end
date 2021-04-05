using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace PetRescue.Data.Domains
{
    public class UserDomain : BaseDomain
    {
        public UserDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public object GetUserDetail(string token)
        {
            var userRepo = uow.GetService<IUserRepository>();
            var userProfileRepo = uow.GetService<IUserProfileRepository>();
            var handler = new JwtSecurityTokenHandler();
            var result = handler.ReadJwtToken(token) as JwtSecurityToken;
            var currentClaims = result.Claims.ToList();
            string email = currentClaims.FirstOrDefault(t => t.Type == "email").Value;
            var user = userRepo.Get().FirstOrDefault(u => u.UserEmail == email);
            if(user == null)
            {
                return new
                {
                    message = "Not Found User"
                };
            }
            var userProfile = userProfileRepo.FindById(user.UserId);
            if(userProfile != null)
            {
                var returnResult = new UserDetailModel
                {
                    Email = user.UserEmail,
                    Id = user.UserId.ToString(),
                    Roles = user.UserRole.Select(r => r.Role.RoleName).ToArray(),
                    CenterId = user.CenterId,
                    Phone = userProfile.Phone,
                    DoB  = userProfile.Dob,
                    FirstName = userProfile.FirstName,
                    Gender = userProfile.Gender,
                    LastName = userProfile.LastName,
                    ImgUrl = userProfile.ImageUrl
                };
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
        public int UpdateUserProfile(UserProfileUpdateModel model)
        {
            var profileRepo = uow.GetService<IUserProfileRepository>();

            var userProfile = profileRepo.FindById(model.UserId);
            var result = userProfile == null ? profileRepo.Create(model) 
                : profileRepo.Edit(userProfile, model);
            uow.saveChanges();
            if(result != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public string AddRoleManagerToUser(UserRoleUpdateModel model, Guid insertBy)
        {
                var userRoleDomain = uow.GetService<UserRoleDomain>();
                var newRole = userRoleDomain.RegistationRole(model.UserId, model.RoleName, insertBy);
                if(newRole != null)
                {
                    return model.UserId.ToString();
                }
            return null;
        }
        public List<NotificationToken> GetListDeviceTokenByRoleAndApplication(string roleName, string applicationName) 
        {
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            var roleRepo = uow.GetService<IRoleRepository>();
            var notificationTokenRepo = uow.GetService<INotificationTokenRepository>();
            var roleId = roleRepo.Get().FirstOrDefault(r => r.RoleName.Equals(roleName)).RoleId;
            var listUserRole = userRoleRepo.Get().Where(s => s.RoleId.Equals(roleId)).ToList();
            var listNotificationToken = new List<NotificationToken>();
            foreach (var userRole in listUserRole)
            {
                var notificationToken = notificationTokenRepo.Get().FirstOrDefault(s => s.UserId.Equals(userRole.UserId) && s.ApplicationName.Equals(applicationName));
                if(notificationToken != null)
                {
                    listNotificationToken.Add(notificationToken);
                }
            }
            return listNotificationToken;
        }
        public NotificationToken GetManagerDeviceTokenByCenterId(Guid centerId)
        {
            var notificationTokenRepo = uow.GetService<INotificationTokenRepository>();
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            var currentUserRole = userRoleRepo.Get().FirstOrDefault(s => s.User.CenterId.Equals(centerId) && s.Role.RoleName.Equals(RoleConstant.MANAGER));
            var notificationToken = notificationTokenRepo.Get().FirstOrDefault(s => s.UserId.Equals(currentUserRole.UserId) && s.ApplicationName.Equals(ApplicationNameHelper.MANAGE_CENTER_APP));
            return notificationToken;
        }
        public string AddUserToCenter(AddNewRoleModel model)
        {
            var userRepo = uow.GetService<IUserRepository>();
            var userRoleDomain = uow.GetService<UserRoleDomain>();
            var currentUser = userRepo.Get().FirstOrDefault(s=> s.UserEmail.Equals(model.Email));
            var result = "";
            if(currentUser != null)
            {
                //find role of User
                var userRole = userRoleDomain.CheckRoleOfUser(new UserRoleUpdateModel
                {
                    RoleName = model.RoleName,
                    UserId = currentUser.UserId
                });
                // if user isn't exist
                if (userRole == null)
                {
                    //if another role is existed
                    if (currentUser.CenterId.Equals(model.CenterId) && (bool)currentUser.IsBelongToCenter)
                    {
                        userRoleDomain.RegistationRole(currentUser.UserId, model.RoleName, model.InsertBy);
                        result = currentUser.UserId.ToString();
                    }
                    // if another role isn't existed
                    else
                    {
                        userRepo.UpdateUserModel(currentUser, new UserUpdateModel
                        {
                            CenterId = model.CenterId,
                            IsBelongToCenter = true
                        });
                        userRoleDomain.RegistationRole(currentUser.UserId, model.RoleName, model.InsertBy);
                        result =  currentUser.UserId.ToString();
                    }
                }
                else
                {
                    if ((bool) !currentUser.IsBelongToCenter)
                    {
                        if (!userRole.IsActive)
                        {
                            userRoleDomain.Edit(userRole, new UserRoleUpdateEntityModel
                            {
                                IsActive = true,
                                UpdateBy = model.InsertBy
                            });
                            userRepo.UpdateUserModel(currentUser, new UserUpdateModel
                            {
                                CenterId = model.CenterId,
                                IsBelongToCenter = true
                            });
                            result = currentUser.UserId.ToString();
                        }
                        else
                        {
                            result = "This Role is existed";
                        }
                    }
                    else
                    {
                        if (currentUser.CenterId.Equals(model.CenterId))
                        {
                            userRoleDomain.Edit(userRole, new UserRoleUpdateEntityModel
                            {
                                IsActive = true,
                                UpdateBy = model.InsertBy
                            });
                            result = currentUser.UserId.ToString();
                        }
                        else
                        {
                            result = "This user is belong another center";
                        }
                    }
                }
            }
            else
            {
                var context = uow.GetService<PetRescueContext>();
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var newCreateUserModel = new UserCreateModel
                        {
                            Email = model.Email,
                            CenterId = model.CenterId,
                            IsBelongToCenter = UserConst.BELONG,
                        };
                        var newUser = userRepo.CreateUserByModel(newCreateUserModel);
                        userRoleDomain.RegistationRole(newUser.UserId, model.RoleName, model.InsertBy);
                        var newUserProfileModel = new UserProfileUpdateModel
                        {
                            DoB = model.DoB,
                            FirstName = model.FirstName,
                            Gender = model.Gender,
                            LastName = model.LastName,
                            Phone = model.Phone,
                            UserId = newUser.UserId,
                        };
                        UpdateUserProfile(newUserProfileModel);
                        transaction.Commit();
                        result = newUser.UserId.ToString();
                    }
                    catch
                    {
                        transaction.Rollback();
                        result="This cannot create";
                    }
                }
            }
            return result;
        }
        public string[] GetRoleOfUser(Guid userId)
        {
            var userRepo = uow.GetService<IUserRepository>();
            var user = userRepo.Get().FirstOrDefault(s=>s.UserId.Equals(userId));
            if (user != null)
            {
                return user.UserRole.Where(s => s.IsActive).Select(r => r.Role.RoleName).ToArray();
            }
            return null;
        }
        public string RemoveVolunteerOfCenter(RemoveVolunteerRoleModel model)
        {
            var userRoleDomain = uow.GetService<UserRoleDomain>();
            var userRepo = uow.GetService<IUserRepository>();
            var context = uow.GetService<PetRescueContext>();
            var notificationTokenDomain = uow.GetService<NotificationTokenDomain>();
            var userRole = userRoleDomain.CheckRoleOfUser(new UserRoleUpdateModel
            {
                RoleName = RoleConstant.VOLUNTEER,
                UserId = model.UserId
            });
            var result = "Not Found";
            if (userRole != null)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try 
                    {
                        var currentUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(model.UserId));
                        userRoleDomain.Edit(userRole, new UserRoleUpdateEntityModel
                        {
                            IsActive = false,
                            UpdateBy = model.InsertBy
                        });
                        var notificationToken = notificationTokenDomain.DeleteNotificationByUserIdAndApplicationName(currentUser.UserId, ApplicationNameHelper.VOLUNTEER_APP);
                        if (GetRoleOfUser(model.UserId).Length == 0)
                        {
                            userRepo.UpdateUserModel(currentUser, new UserUpdateModel
                            {
                                CenterId = model.CenterId,
                                IsBelongToCenter = false
                            });
                        }
                        uow.saveChanges();
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
            uow.saveChanges();
            return result;
        }
        public List<UserProfileViewModel> GetListProfileOfVolunter(Guid centerId) 
        {
            var userRepository = uow.GetService<IUserRepository>();
            var userRoleRepository = uow.GetService<IUserRoleRepository>();
            var listUser =userRepository.Get().Where(s => s.CenterId.Equals(centerId));
            var result = new List<UserProfileViewModel>();
            foreach(var user in listUser)
            {
                var model = new UserRoleUpdateModel
                {
                    UserId = user.UserId,
                    CenterId = (Guid)user.CenterId,
                    RoleName = RoleConstant.VOLUNTEER
                };
                if(userRoleRepository.FindUserRoleByUserRoleUpdateModel(model) != null)
                {
                    result.Add(new UserProfileViewModel 
                    {
                        email = user.UserEmail,
                        DoB = user.UserProfile.Dob,
                        FirstName = user.UserProfile.FirstName,
                        Gender = user.UserProfile.Gender,
                        ImgUrl = user.UserProfile.ImageUrl,
                        LastName = user.UserProfile.LastName,
                        Phone = user.UserProfile.Phone,
                        UserId = user.UserId
                    });
                }
            }
            return result.Count > 0 ? result : null;
        }
        public UserProfileViewModel GetProfileByUserId(Guid userId)
        {
            var userRepo = uow.GetService<IUserRepository>();
            var user = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(userId));
            if(user != null)
            {
                return new UserProfileViewModel
                {
                    email = user.UserEmail,
                    DoB = user.UserProfile.Dob,
                    FirstName = user.UserProfile.FirstName,
                    Gender = user.UserProfile.Gender,
                    ImgUrl = user.UserProfile.ImageUrl,
                    LastName = user.UserProfile.LastName,
                    Phone = user.UserProfile.Phone,
                    UserId = user.UserId
                };
            }
            return null;
        }
    }
}
