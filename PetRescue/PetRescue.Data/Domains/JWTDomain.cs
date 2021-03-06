﻿using Microsoft.IdentityModel.Tokens;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
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
            User user = UserIsExisted(email);
            if (user != null)
            {
                var tokenDescriptor = GeneratedTokenDecriptor(email, user.UserId.ToString(), currentClaims);
                var newToken = handler.CreateToken((SecurityTokenDescriptor)tokenDescriptor);
                return handler.WriteToken(newToken);
            }
            else
            {
                var userRepo = uow.GetService<IUserRepository>();
                user = userRepo.CreateUser(email);
                if(user != null)
                {
                        var tokenDescriptor = GeneratedTokenDecriptor(email, user.UserId.ToString(), currentClaims);
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
        private object GeneratedTokenDecriptor(string email, string userId, List<Claim> currentClaims)
        {
            string[] roles = GetRoleUser(email);
            if (roles != null)
            {
                foreach (string role in roles)
                {
                    Claim newClaim = new Claim(ClaimTypes.Role, role);
                    currentClaims.Add(newClaim);
                }
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
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenDescriptor;
        }

    }
}
