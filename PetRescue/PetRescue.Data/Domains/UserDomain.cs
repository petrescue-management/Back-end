using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class UserDomain : BaseDomain
    {
        public UserDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public string RegisterUser(string email)
        {
            var userRepo = uow.GetService<IUserRepository>();
            var newUser = userRepo.CreateUser(email);
            return newUser.UserId.ToString();
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
            bool isHasProfile = userProfileRepo.Get().FirstOrDefault(u => u.UserId == user.UserId) != null;
            var fullname = "";
            if (isHasProfile)
            {
                fullname = userProfileRepo.Get().FirstOrDefault(u => u.UserId == user.UserId).LastName + " ";
                fullname += userProfileRepo.Get().FirstOrDefault(u => u.UserId == user.UserId).FirstName;
            }
            
            var returnResult = new UserDetailModel
            {
                Email = user.UserEmail,
                Id = user.UserId.ToString(),
                Roles = user.UserRole.Where(r => r.IsActived == true).Select(r => r.Role.RoleName).ToArray(),
                FullName = fullname
            };
            return returnResult;
        }
    }
}
