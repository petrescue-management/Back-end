
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
    public class JWTDomain : BaseDomain
    {
      
        public JWTDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public object DecodeJwt(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var result = handler.ReadJwtToken(jwt) as JwtSecurityToken;
            var currentClaims = result.Claims.ToList();
            string email = currentClaims.FirstOrDefault(c => c.Type == "email").Value;
            string urlImg = currentClaims.FirstOrDefault(c => c.Type == "picture").Value;
            string fullName = currentClaims.FirstOrDefault(c => c.Type == "name").Value;
            string[] listStr = fullName.Split(" ");
            string lastName = "";
            string firstName = "";
            for(int index =0; index < listStr.Length; index++)
            {
                if(index == 0)
                {
                    lastName += listStr[0];
                }
                else
                {
                    firstName += " " + listStr[index];
                }
            }
            User user = UserIsExisted(email);
            if (user != null)
            {
                var tokenDescriptor = GeneratedTokenDecriptor(user, currentClaims);
                var newToken = handler.CreateToken((SecurityTokenDescriptor)tokenDescriptor);
                return handler.WriteToken(newToken);
            }
            else
            {
                var userRepo = uow.GetService<IUserRepository>();
                var userProfileRepo = uow.GetService<IUserProfileRepository>();
                var newUserModel = new UserCreateByAppModel
                {
                    Email = email,    
                };
                user = userRepo.CreateUser(newUserModel);
                if(user != null)
                {
                    var newUpdateProfileModel = new UserProfileUpdateModel
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        UserId = user.UserId,
                        Address= "",
                        DoB = DateTime.UtcNow,
                        Gender = 0,
                        Phone = "",
                        ImgUrl = urlImg
                    };
                    userProfileRepo.Create(newUpdateProfileModel);
                    var tokenDescriptor = GeneratedTokenDecriptor(user, currentClaims);
                    var newToken = handler.CreateToken((SecurityTokenDescriptor)tokenDescriptor);
                    return handler.WriteToken(newToken);
                }
                return null;
            }
           
        }
        private User UserIsExisted(string email)
        {
            var userRepo = uow.GetService<IUserRepository>();
            return userRepo.FindById(email);
        }
        private string[] GetRoleUser(string email)
        {
            var userRepo = uow.GetService<IUserRepository>();
            var roles = userRepo.FindById(email);
            if(roles != null)
            {
                return roles.UserRole.Select(r => r.Role.RoleName).ToArray();
            }
            return null;
        }
        private object GeneratedTokenDecriptor(User currentUser, List<Claim> currentClaims)
        {
            string[] roles = GetRoleUser(currentUser.UserEmail);
            if (roles != null)
            {
                foreach (string role in roles)
                {
                    Claim newClaim = new Claim(ClaimTypes.Role, role);
                    currentClaims.Add(newClaim);
                }
            }
            currentClaims.Add(new Claim(ClaimTypes.Actor, currentUser.UserId.ToString()));
            if(ValidationExtensions.IsNotNull(currentUser.CenterId))
            {
                currentClaims.Add(new Claim("centerId", currentUser.CenterId.ToString()));
            }
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(currentClaims);
            var key = Encoding.ASCII.GetBytes("Sercret_Key_PetRescue");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = "PetRescue_Issuer",
                Audience = "PetRescue_Audience",
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenDescriptor;
        }
        private string GenarateHash(string UserPassword)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(UserPassword);
            SHA256Managed PasswordHash = new SHA256Managed();

            byte[] hash = PasswordHash.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        public string LoginBySysAdmin(UserLoginBySysadminModel model)
        {
            string hashedPassword = GenarateHash(model.Password);
            var userRepo = uow.GetService<IUserRepository>();
            var jwtDomain = uow.GetService<JWTDomain>();
            var currentUser = userRepo.FindById(model.Email);
            if (currentUser != null)
            {
                if (currentUser.Password.Equals(hashedPassword))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var result = new JwtSecurityToken();
                    var currentClaims = result.Claims.ToList();
                    currentClaims.Add(new Claim(ClaimTypes.Email, currentUser.UserEmail));
                    var tokenDescriptor = jwtDomain.GeneratedTokenDecriptor(currentUser, currentClaims);
                    var newToken = handler.CreateToken((SecurityTokenDescriptor)tokenDescriptor);
                    return handler.WriteToken(newToken);
                }
                return null;
            }
            return null;
        }

    }
}
