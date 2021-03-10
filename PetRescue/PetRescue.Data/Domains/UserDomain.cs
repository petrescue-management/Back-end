using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class UserDomain : BaseDomain
    {
        public UserDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public User RegisterUser(UserCreateByAppModel model)
        {
            var userRepo = uow.GetService<IUserRepository>();
            var newUser = userRepo.CreateUser(model);
            return newUser;
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
        public UserProfile UpdateUserProfile(UserProfileUpdateModel model)
        {
            var profileRepo = uow.GetService<IUserProfileRepository>();

            var userProfile = profileRepo.FindById(model.UserId);
            if(userProfile == null)
            {
                var result = profileRepo.Create(model);
                return result;
            }
            else
            {
                var result = profileRepo.Edit(userProfile, model);
                return result;
            }
        }
        public User GetUserById(Guid userId)
        {
            var userRepo = uow.GetService<IUserRepository>();
            return userRepo.FindById(null,userId.ToString());
        }

        public User UpdateCenter(UserUpdateCenterModel model, User currentUser)
        {
            var userRepo = uow.GetService<IUserRepository>();
            if(currentUser != null)
            {
                var newuser = userRepo.Edit(currentUser,model.CenterId);
                return userRepo.Update(newuser).Entity;
            }
            return null;
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
        private string GenarateHash(string UserPassword)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(UserPassword);
            SHA256Managed PasswordHash = new SHA256Managed();

            byte[] hash = PasswordHash.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
        public bool LoginBySysAdmin (UserLoginBySysadminModel model)
        {
            string hashedPassword = GenarateHash(model.Password);
            var userRepo = uow.GetService<IUserRepository>();
            var currentUser = userRepo.FindById(model.Email);
            if (currentUser != null)
            {
                if (currentUser.Password.Equals(hashedPassword))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        //public User AddRoleToUser(UserRoleUpdateModel model)
        //{
        //    var currentUser = GetUserById(model.UserId);
        //    return null;
        //    // get current user
        //    //          if (currentUser == null) // current user not found
        //    //{
        //    //    return null;
        //    //}//end of if
        //    //if (currentUser.IsBelongToCenter.Value)
        //    //{
        //    //    var userRoleDomain = uow.GetService<UserRoleDomain>();
        //    //    var userRole = userRoleDomain.IsExisted(model);
        //    //    if (userRole == null)
        //    //    {
        //    //        var newUserRole = userRoleDomain.RegistationRole(model.UserId, model.RoleName);
        //    //        if (newUserRole != null)
        //    //        {
        //    //            return currentUser;
        //    //        }
        //    //        return null;
        //    //    }
        //    //    return null;
        //    //}//end of if
        //    //else
        //    //{
        //    //    var userRoleDomain = uow.GetService<UserRoleDomain>();
        //    //    var userRole = userRoleDomain.IsExisted(model);
        //    //    if (userRole == null)
        //    //    {
        //    //        var tempModel = new UserUpdateCenterModel
        //    //        {
        //    //            CenterId = model.CenterId,
        //    //            UserId = model.UserId
        //    //        };
        //    //        var tempUser = UpdateCenter(tempModel, currentUser);
        //    //        if (tempUser != null)
        //    //        {
        //    //            var newUserRole = userRoleDomain.RegistationRole(model.UserId, model.RoleName);
        //    //            if (newUserRole != null)
        //    //            {
        //    //                return tempUser;
        //    //            }
        //    //            return null;
        //    //        }
        //    //        return null;  
        //    //    }
        //    //    else
        //    //    {
        //    //        var tempModel = new UserUpdateCenterModel
        //    //        {
        //    //            CenterId = model.CenterId,
        //    //            UserId = model.UserId
        //    //        };
        //    //        var tempUser = UpdateCenter(tempModel, currentUser);
        //    //        if (tempUser != null)
        //    //        {
        //    //            return tempUser;
        //    //        }
        //    //        return null;
        //    //    }
        //    //}//end of else
        //}
    }
}
