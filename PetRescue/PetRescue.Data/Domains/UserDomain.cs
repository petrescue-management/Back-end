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
        private readonly IWorkingHistoryRepository _workingHistoryRepo;
        private readonly UserRoleDomain _userRoleDomain;
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly INotificationTokenRepository _notificationTokenRepo;
        private readonly DbContext _context;
        private readonly NotificationTokenDomain _notificationTokenDomain;
        public UserDomain(IUnitOfWork uow, IUserProfileRepository userProfileRepo, IUserRepository userRepo, ICenterRepository centerRepo, IWorkingHistoryRepository workingHistoryRepo, UserRoleDomain userRoleDomain, IRoleRepository roleRepo, IUserRoleRepository userRoleRepo, INotificationTokenRepository notificationTokenRepo, DbContext context, NotificationTokenDomain notificationTokenDomain) : base(uow)
        {
            this._userProfileRepo = userProfileRepo;
            this._userRepo = userRepo;
            this._centerRepo = centerRepo;
            this._workingHistoryRepo = workingHistoryRepo;
            this._userRoleDomain = userRoleDomain;
            this._roleRepo = roleRepo;
            this._userRoleRepo = userRoleRepo;
            this._notificationTokenRepo = notificationTokenRepo;
            this._context = context;
            this._notificationTokenDomain = notificationTokenDomain;
        }
        public object GetUserDetail(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var result = handler.ReadJwtToken(token) as JwtSecurityToken;
            var currentClaims = result.Claims.ToList();
            string email = currentClaims.FirstOrDefault(t => t.Type == "email").Value;
            var user = _userRepo.Get().FirstOrDefault(u => u.UserEmail == email);
            var workingHistory = _workingHistoryRepo.Get().FirstOrDefault(s => s.UserId.Equals(user.UserId) && s.IsActive == true);
            var centerId = Guid.Empty;
            if (workingHistory != null)
            {
                centerId = workingHistory.CenterId;
            }
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
                    Roles = user.UserRole.Where(r => r.IsActive).Select(r => r.Role.RoleName).ToArray(),
                    CenterId = centerId,
                    Phone = userProfile.Phone,
                    DoB = userProfile.Dob,
                    FirstName = userProfile.FirstName,
                    Gender = userProfile.Gender,
                    LastName = userProfile.LastName,
                    ImgUrl = userProfile.ImageUrl,
                    UpdatedAt = userProfile.UpdatedAt
                };
                if ((bool)user.IsBelongToCenter)
                {
                    var center = _centerRepo.Get().FirstOrDefault(s => s.CenterId.Equals(centerId));
                    returnResult.Center = new CenterProfileViewModel
                    {
                        CenterAddrress = center.Address,
                        CenterName = center.CenterName
                    };
                }
                return returnResult;
            }
            else
            {
                var returnResult = new UserDetailModel
                {
                    Email = user.UserEmail,
                    Id = user.UserId.ToString(),
                    Roles = user.UserRole.Select(r => r.Role.RoleName).ToArray(),
                    CenterId = centerId,
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
                _uow.saveChanges();
                return true;
            }
            return false;
        }
        public string AddRoleManagerToUser(UserRoleUpdateModel model, Guid insertBy)
        {
            var newRole = _userRoleDomain.RegistationRole(model.UserId, model.RoleName, insertBy);
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
            var working = _workingHistoryRepo.Get().FirstOrDefault(s => s.CenterId.Equals(centerId) && s.RoleName.Equals(RoleConstant.MANAGER) && s.IsActive);
            var notificationToken = _notificationTokenRepo.Get().FirstOrDefault(s => s.UserId.Equals(working.UserId) && s.ApplicationName.Equals(ApplicationNameHelper.MANAGE_CENTER_APP));
            return notificationToken;
        }
        public string AddVolunteerToCenter(AddNewRoleModel model)
        {
            var result = AddUserToCenter(model);
            if (!result.Contains("This")) {
                _workingHistoryRepo.Create(new WorkingHistoryCreateModel
                {
                    CenterId = model.CenterId,
                    Description = "",
                    RoleName = RoleConstant.VOLUNTEER,
                    UserId = Guid.Parse(result)
                });
                _uow.saveChanges();
                return result;
            }
            return result;
        }
        public string AddUserToCenter(AddNewRoleModel model)
        {
            var currentUser = _userRepo.Get().FirstOrDefault(s => s.UserEmail.Equals(model.Email));
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
                    //if another role is existed
                    if ((bool)currentUser.IsBelongToCenter)
                    {
                        _userRoleDomain.RegistationRole(currentUser.UserId, model.RoleName, model.InsertBy);
                        result = currentUser.UserId.ToString();
                    }
                    // if another role isn't existed
                    else
                    {
                        _userRepo.UpdateUserModel(currentUser, new UserUpdateModel
                        {
                            IsBelongToCenter = true
                        });
                        _userRoleDomain.RegistationRole(currentUser.UserId, model.RoleName, model.InsertBy);
                        result = currentUser.UserId.ToString();
                    }
                }
                else
                {
                    if ((bool)!currentUser.IsBelongToCenter)
                    {
                        if (!userRole.IsActive)
                        {
                            _userRoleDomain.Edit(userRole, new UserRoleUpdateEntityModel
                            {
                                IsActive = true,
                                UpdateBy = model.InsertBy
                            });
                            _userRepo.UpdateUserModel(currentUser, new UserUpdateModel
                            {
                                //CenterId = model.CenterId,
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
                        if (!userRole.IsActive)
                        {
                            _userRoleDomain.Edit(userRole, new UserRoleUpdateEntityModel
                            {
                                IsActive = true,
                                UpdateBy = model.InsertBy
                            });
                            result = currentUser.UserId.ToString();
                        }
                        else
                        {
                            if (currentUser.WorkingHistory.FirstOrDefault(s => s.IsActive).CenterId.Equals(model.CenterId))
                            {
                                result = "This role is existed";
                            }
                            else
                            {
                                result = "This user is belong another center";
                            }
                        }
                    }
                }
            }
            else
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var newCreateUserModel = new UserCreateModel
                        {
                            Email = model.Email,
                            IsBelongToCenter = UserConst.BELONG,
                        };
                        var newUser = _userRepo.CreateUserByModel(newCreateUserModel);
                        _userRoleDomain.RegistationRole(newUser.UserId, model.RoleName, model.InsertBy);
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
                return user.UserRole.Where(s => s.IsActive).Select(r => r.Role.RoleName).ToArray();
            }
            return null;
        }
        public async Task<string> RemoveVolunteerOfCenter(RemoveVolunteerRoleModel model, string path)
        {
            
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
                            IsActive = false,
                            UpdateBy = model.InsertBy
                        });
                        var workingHistory = _workingHistoryRepo.Get().FirstOrDefault(s => s.UserId.Equals(currentUser.UserId) && s.RoleName.Equals(RoleConstant.VOLUNTEER) && s.IsActive == true);
                        _workingHistoryRepo.Edit(new WorkingHistoryUpdateModel
                        {
                            Description = model.Description,
                            IsActive = false,
                            WorkingHistoryId = workingHistory.WorkingHistoryId
                        });
                        _notificationTokenDomain.DeleteNotificationByUserIdAndApplicationName(currentUser.UserId, ApplicationNameHelper.VOLUNTEER_APP);
                        if (GetRoleOfUser(model.UserId).Length == 0)
                        {
                            _userRepo.UpdateUserModel(currentUser, new UserUpdateModel
                            {
                                IsBelongToCenter = false
                            });
                        }
                        var firebaseExtensions = new FireBaseExtentions();
                        var app = firebaseExtensions.GetFirebaseApp(path);
                        var fcm = FirebaseMessaging.GetMessaging(app);
                        var notificationToken = _notificationTokenDomain.FindByApplicationNameAndUserId(ApplicationNameHelper.VOLUNTEER_APP, currentUser.UserId);
                        if(notificationToken != null)
                        {
                            var listToken = new List<string>();
                            listToken.Add(notificationToken.DeviceToken);
                            await fcm.UnsubscribeFromTopicAsync(listToken, model.CenterId.ToString());
                        }
                        _uow.saveChanges();
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
        public object GetListProfileOfVolunter(Guid centerId, bool isActive)
        {
            var workings = _workingHistoryRepo.Get().Where(s => s.CenterId.Equals(centerId) && s.RoleName.Equals(RoleConstant.VOLUNTEER) && s.IsActive == isActive).ToList();
            var result = new List<UserProfileViewModel2>();
            foreach (var working in workings)
            {
                var user = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(working.UserId));
                result.Add(new UserProfileViewModel2
                {
                    Email = user.UserEmail,
                    DoB = user.UserProfile.Dob,
                    FirstName = user.UserProfile.FirstName,
                    Gender = user.UserProfile.Gender,
                    ImgUrl = user.UserProfile.ImageUrl,
                    LastName = user.UserProfile.LastName,
                    Phone = user.UserProfile.Phone,
                    UserId = user.UserId,
                    DateStarted = working.DateStarted,
                    DateEnded = working.DateEnded
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
                        ImgUrl = user.UserProfile.ImageUrl,
                        LastName = user.UserProfile.LastName,
                        Phone = user.UserProfile.Phone,
                        UserId = user.UserId
                    });
                }
                var result = new Dictionary<string, object>();
                result["totalPages"] = total;
                result["result"] = listUsers;
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
                    ImgUrl = user.UserProfile.ImageUrl,
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
