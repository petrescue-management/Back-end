﻿
using FirebaseAdmin.Messaging;
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
using System.Threading.Tasks;

namespace PetRescue.Data.Domains
{
    public class JWTDomain : BaseDomain
    {
        private readonly NotificationTokenDomain _notificationTokenDomain;
        private readonly IUserProfileRepository _userProfileRepo;
        private readonly IUserRepository _userRepo;
        private readonly IWorkingHistoryRepository _workingHistoryRepo;
        private readonly UserDomain _userDomain;
        public JWTDomain(IUnitOfWork uow, NotificationTokenDomain notificationTokenDomain, IUserProfileRepository userProfileRepo, IUserRepository userRepo, IWorkingHistoryRepository workingHistoryRepo, UserDomain userDomain) : base(uow)
        {
            this._notificationTokenDomain = notificationTokenDomain;
            this._userProfileRepo = userProfileRepo;
            this._userRepo = userRepo;
            this._workingHistoryRepo = workingHistoryRepo;
            this._userDomain = userDomain;
        }
        public JWTReturnModel DecodeJwt(UserLoginModel model)
        {
            var temp = new NotificationToken();
            var handler = new JwtSecurityTokenHandler();
            var result = handler.ReadJwtToken(model.Token) as JwtSecurityToken;
            var currentClaims = result.Claims.ToList();
            string email = currentClaims.FirstOrDefault(c => c.Type == "email").Value;
            string urlImg = currentClaims.FirstOrDefault(c => c.Type == "picture").Value;
            string fullName = currentClaims.FirstOrDefault(c => c.Type == "name").Value;
            string[] listStr = fullName.Split(" ");
            string lastName = "";
            string firstName = "";
            var returnResult = new JWTReturnModel();
            for (int index =0; index < listStr.Length; index++)
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
                var notificationToken = _notificationTokenDomain.FindByApplicationNameAndUserId(model.ApplicationName, user.UserId);
                var tokenDescriptor = GeneratedTokenDecriptor(user, currentClaims);
                var newToken = handler.CreateToken((SecurityTokenDescriptor)tokenDescriptor);
                //if notification Token is existed,  will update deviceToken
                if (notificationToken != null) 
                {
                    temp = _notificationTokenDomain.UpdateNotificationToken(new NotificationTokenUpdateModel
                    {
                        Id = notificationToken.Id,
                        DeviceToken = model.DeviceToken
                    });
                }
                // else create new notificationToken.
                else
                {
                    temp = _notificationTokenDomain.CreateNotificationToken(new NotificationTokenCreateModel
                    {
                        ApplicationName = model.ApplicationName,
                        DeviceToken = model.DeviceToken,
                        UserId = user.UserId
                    });
                }
                if(_userProfileRepo.Get().FirstOrDefault(s=> s.UserId.Equals(user.UserId)) == null){
                    var newUpdateProfileModel = new UserProfileUpdateModel
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        UserId = user.UserId,
                        DoB = DateTime.UtcNow,
                        Gender = 3,
                        Phone = "",
                        ImgUrl = urlImg
                    };
                    _userProfileRepo.Create(newUpdateProfileModel);
                }
                _uow.saveChanges();
                returnResult.Jwt = handler.WriteToken(newToken);
                returnResult.NotificationToken = temp;
                return returnResult;
            }
            else
            {
                var newUserModel = new UserCreateByAppModel
                {
                    Email = email,    
                };
                user = _userRepo.CreateUser(newUserModel);
                if(user != null)
                {
                    //create new Profile
                    var newUpdateProfileModel = new UserProfileUpdateModel
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        UserId = user.UserId,
                        DoB = DateTime.UtcNow,
                        Gender = 3,
                        Phone = "",
                        ImgUrl = urlImg
                    };
                    _userProfileRepo.Create(newUpdateProfileModel);
                    //create new notification token
                    temp = _notificationTokenDomain.CreateNotificationToken(new NotificationTokenCreateModel
                    {
                        ApplicationName = model.ApplicationName,
                        DeviceToken = model.DeviceToken,
                        UserId = user.UserId
                    });
                    var tokenDescriptor = GeneratedTokenDecriptor(user, currentClaims);
                    var newToken = handler.CreateToken((SecurityTokenDescriptor)tokenDescriptor);
                    _uow.saveChanges();
                    returnResult.Jwt = handler.WriteToken(newToken);
                    returnResult.NotificationToken = temp;
                    return returnResult;
                }
                return null;
            }
           
        }
        private User UserIsExisted(string email)
        {
            return _userRepo.FindById(email);
        }
        private string[] GetRoleUser(string email)
        {
            var user = _userRepo.FindById(email);
            if(user != null)
            {
                return user.UserRole.Where(s=>s.IsActive).Select(r => r.Role.RoleName).ToArray();
            }
            return null;
        }
        private object GeneratedTokenDecriptor(User currentUser, List<Claim> currentClaims)
        {
    
            string[] roles = GetRoleUser(currentUser.UserEmail);
            var working = _workingHistoryRepo.Get().FirstOrDefault(s => s.UserId.Equals(currentUser.UserId) && s.IsActive);
            if (roles != null)
            {
                foreach (string role in roles)
                {
                    Claim newClaim = new Claim(ClaimTypes.Role, role);
                    currentClaims.Add(newClaim);
                }
            }
            currentClaims.Add(new Claim(ClaimTypes.Actor, currentUser.UserId.ToString()));
            if(working != null)
            {
                if (ValidationExtensions.IsNotNull(working.CenterId))
                {
                    currentClaims.Add(new Claim("centerId", working.CenterId.ToString()));
                }
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
            var currentUser = _userRepo.FindById(model.Email);
            if (currentUser != null)
            {
                if (currentUser.Password.Equals(hashedPassword))
                {
                    var currentNotificationToken = _notificationTokenDomain.FindByApplicationNameAndUserId(model.ApplicationName, currentUser.UserId);
                    if (currentNotificationToken == null) {
                        _notificationTokenDomain.CreateNotificationToken(new NotificationTokenCreateModel
                        {
                            UserId = currentUser.UserId,
                            ApplicationName = model.ApplicationName,
                            DeviceToken = model.DeviceToken
                        });
                    }
                    else
                    {
                        if (!currentNotificationToken.DeviceToken.Equals(model.DeviceToken))
                        {
                            _notificationTokenDomain.UpdateNotificationToken(new NotificationTokenUpdateModel
                            {
                                Id = currentNotificationToken.Id,
                                DeviceToken = model.DeviceToken
                            });
                        }
                    }
                    //Generator token for user
                    var handler = new JwtSecurityTokenHandler();
                    var result = new JwtSecurityToken();
                    var currentClaims = result.Claims.ToList();
                    currentClaims.Add(new Claim(ClaimTypes.Email, currentUser.UserEmail));
                    var tokenDescriptor = GeneratedTokenDecriptor(currentUser, currentClaims);
                    var newToken = handler.CreateToken((SecurityTokenDescriptor)tokenDescriptor);
                    _uow.saveChanges();
                    return handler.WriteToken(newToken);
                }
                return null;
            }
            return null;
        }
        public async Task<string> LoginByVolunteer(UserLoginModel model, string path)
        {
            //Get FirebaseToken and get claims
            var handler = new JwtSecurityTokenHandler();
            var result = handler.ReadJwtToken(model.Token) as JwtSecurityToken;
            var currentClaims = result.Claims.ToList();
            //Start :Get Information from claims
            string email = currentClaims.FirstOrDefault(c => c.Type == "email").Value;
            var user = UserIsExisted(email);
            // check this user
            if(user != null)
            {
                string[] roles = _userDomain.GetRoleOfUser(user.UserId);
                bool check = false;
                for (int index = 0; index < roles.Length; index++)
                {
                    if (roles[index].Equals(RoleConstant.VOLUNTEER))
                    {
                        check = true;
                    }
                }
                if (check)
                {
                    string urlImg = currentClaims.FirstOrDefault(c => c.Type == "picture").Value;
                    string fullName = currentClaims.FirstOrDefault(c => c.Type == "name").Value;
                    string[] listStr = fullName.Split(" ");
                    string lastName = "";
                    string firstName = "";
                    var returnResult = new JWTReturnModel();
                    for (int index = 0; index < listStr.Length; index++)
                    {
                        if (index == 0)
                        {
                            lastName += listStr[0];
                        }
                        else
                        {
                            firstName += " " + listStr[index];
                        }
                    }
                    //End :Get Information from FirebaseToken
                    var temp = new NotificationToken();
                    var tokenDescriptor = GeneratedTokenDecriptor(user, currentClaims);
                    var newToken = handler.CreateToken((SecurityTokenDescriptor)tokenDescriptor);
                    var firebaseExtensions = new FireBaseExtentions();
                    var app = firebaseExtensions.GetFirebaseApp(path);
                    var fcm = FirebaseMessaging.GetMessaging(app);
                    var notificationToken = _notificationTokenDomain.FindByApplicationNameAndUserId(model.ApplicationName, user.UserId);
                    var centerId = _workingHistoryRepo.Get().FirstOrDefault(s => s.UserId.Equals(user.UserId) && s.RoleName.Equals(RoleConstant.VOLUNTEER) && s.IsActive == true).CenterId;
                    var listToken = new List<string>();
                    //if notification Token is existed,  will update deviceToken
                    if (notificationToken != null)
                    {
                        try
                        {
                            listToken.Add(notificationToken.DeviceToken);
                            await fcm.UnsubscribeFromTopicAsync(listToken, centerId.ToString());
                            temp = _notificationTokenDomain.UpdateNotificationToken(new NotificationTokenUpdateModel
                            {
                                Id = notificationToken.Id,
                                DeviceToken = model.DeviceToken
                            });
                            listToken.Clear();
                            listToken.Add(temp.DeviceToken);
                            await fcm.SubscribeToTopicAsync(listToken, centerId.ToString());
                        }catch(Exception ex)
                        {
                            throw (ex);
                        }
                        
                    }
                    // else create new notificationToken.
                    else
                    {
                        temp = _notificationTokenDomain.CreateNotificationToken(new NotificationTokenCreateModel
                        {
                            ApplicationName = model.ApplicationName,
                            DeviceToken = model.DeviceToken,
                            UserId = user.UserId
                        });
                        listToken.Add(temp.DeviceToken);
                        await fcm.SubscribeToTopicAsync(listToken, centerId.ToString());
                    }
                    app.Delete();
                    _uow.saveChanges();
                    return handler.WriteToken(newToken);
                }
            }
            return null;
        }
    }
}
