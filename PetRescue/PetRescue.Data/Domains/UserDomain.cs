using Microsoft.IdentityModel.Tokens;
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
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
                    Address = userProfile.Address,
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
        public UserProfileViewModel UpdateUserProfile(UserProfileUpdateModel model)
        {
            var profileRepo = uow.GetService<IUserProfileRepository>();

            var userProfile = profileRepo.FindById(model.UserId);
            var result = userProfile != null ? profileRepo.Create(model) 
                : profileRepo.Edit(userProfile, model);
            uow.saveChanges();
            return new UserProfileViewModel 
            {
                Address = result.Address,
                DoB = result.Dob,
                FirstName = result.FirstName,
                Gender= result.Gender,
                ImgUrl= result.ImageUrl,
                LastName = result.LastName,
                Phone = result.Phone,
                UserId = result.UserId
            };
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
            var currentUserRole = userRoleRepo.Get().FirstOrDefault(s => s.User.CenterId.Equals(centerId) && s.Role.RoleName == RoleConstant.MANAGER);
            var notificationToken = notificationTokenRepo.Get().FirstOrDefault(s => s.UserId.Equals(currentUserRole.UserId) && s.ApplicationName.Equals(ApplicationNameHelper.MANAGE_CENTER_APP));
            return notificationToken;
        }
        public User AddUserToCenter(AddNewRoleModel model)
        {
            var userRepo = uow.GetService<IUserRepository>();
            var userRoleDomain = uow.GetService<UserRoleDomain>();
            var currentUser = userRepo.Get().FirstOrDefault(s=> s.UserEmail.Equals(model.Email));
            if(currentUser != null)
            {
                if( (bool)!currentUser.IsBelongToCenter && currentUser.CenterId != null)
                {
                    userRoleDomain.RegistationRole(currentUser.UserId, model.RoleName, model.CenterId);
                }
                else
                {
                    if (currentUser.CenterId.Equals(model.CenterId))
                    {

                    }
                }
            }
            return currentUser;
        }
       
    }
}
