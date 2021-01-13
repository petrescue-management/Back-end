﻿using Microsoft.IdentityModel.Tokens;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
            string userId = UserIsExisted(email);
            if (userId != null)
            {
                string[] roles = GetRoleUser(email);
                foreach(string role in roles)
                {
                    Claim newClaim = new Claim(ClaimTypes.Role, role);
                    currentClaims.Add(newClaim);
                }
                currentClaims.Add(new Claim(ClaimTypes.Actor, userId));
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(currentClaims);
                var key = Encoding.ASCII.GetBytes("Sercret_Key_PetRescue");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Issuer = "PetRescue_Issuer",
                    Audience = "PetRescue_Audience",
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials( new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var newToken = handler.CreateToken(tokenDescriptor);
                return handler.WriteToken(newToken);
            }
            return "User not found";        
        }
        private string UserIsExisted(string email)
        {
            var userRepo = uow.GetService<IUserRepository>();
            return userRepo.FindById(email).UserId.ToString();
        }
        private string[] GetRoleUser(string email)
        {
            var userRepo = uow.GetService<IUserRepository>();
            var roles = userRepo.FindById(email).UserRole.Where(r =>r.IsActived == true).Select(r => r.Role.RoleName).ToArray();
            return roles;
        }
    }
}
