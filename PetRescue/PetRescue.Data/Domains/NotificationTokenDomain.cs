using FirebaseAdmin.Messaging;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Extensions;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetRescue.Data.Domains
{
    public class NotificationTokenDomain : BaseDomain
    {
        public NotificationTokenDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public NotificationToken CreateNotificationToken(NotificationTokenCreateModel model)
        {
            var notificationRepo = uow.GetService<INotificationTokenRepository>();
            return notificationRepo.Create(model);
        }
        public NotificationToken UpdateNotificationToken(NotificationTokenUpdateModel model)
        {
            var notificationRepo = uow.GetService<INotificationTokenRepository>();
            var currentNotificationToken = notificationRepo.GetNotificationTokenById(model.Id);
            return notificationRepo.Edit(currentNotificationToken, model);
        }
        public NotificationToken DeleteNotificationToken(NotificationToken notificationToken)
        {
            var notificationRepo = uow.GetService<INotificationTokenRepository>();
            return notificationRepo.Delete(notificationToken);
        }
        public NotificationToken GetById(Guid notificationId)
        {
            var notificationRepo = uow.GetService<INotificationTokenRepository>();
            return notificationRepo.GetNotificationTokenById(notificationId);
        }
        public bool DeleteNotificationByUserIdAndApplicationName(Guid userId, string applicationName)
        {
            var notificationTokenRepo = uow.GetService<INotificationTokenRepository>();
            var notificationToken = notificationTokenRepo.Get().FirstOrDefault(s => s.UserId.Equals(userId) && s.ApplicationName.Equals(applicationName));
            var result = notificationTokenRepo.Delete(notificationToken);
            if(result != null)
            {
                return true;
            }
            return false;
        }
        public NotificationToken FindByApplicationNameAndUserId(string applicationName,Guid userId)
        {
            var notificationRepo = uow.GetService<INotificationTokenRepository>();
            var notificationToken = notificationRepo.Get().FirstOrDefault(s => s.UserId == userId && s.ApplicationName.Equals(applicationName) && s.IsActive);
            return notificationToken;
        }
        public List<string> FindDeviceTokenByApplicationNameAndRoleNameAndCenterId(string applicationName,string roleName, Guid centerId)
        {
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var notificationTokenRepo = uow.GetService<INotificationTokenRepository>();
            var users = userRepo.Get().Where(s => s.CenterId.Equals(centerId) && (bool)s.IsBelongToCenter).Select(s => s.UserId);
            var listId = new List<Guid>();
            var result = new List<string>();
            foreach(var user in users)
            {
                var userRole = userRoleRepo.Get().FirstOrDefault(s => s.UserId.Equals(user) && s.IsActive && s.Role.RoleName.Equals(roleName));
                if(userRole != null)
                {
                    listId.Add(userRole.UserId);
                }
            };
            foreach(var id in listId)
            {
                var temp = notificationTokenRepo.Get().FirstOrDefault(s => s.ApplicationName.Equals(applicationName) && s.UserId.Equals(id) && s.IsActive);
                if (temp != null)
                {
                    result.Add(temp.DeviceToken);
                }    
            }
            return result;
        }
        public async Task<bool> NotificationForAdminWhenHaveNewCenterRegisterForm(string path)
        {
            try
            {
                var userDomain = uow.GetService<UserDomain>();
                var listToken = userDomain.GetListDeviceTokenByRoleAndApplication(RoleConstant.ADMIN, ApplicationNameHelper.SYSTEM_ADMIN_APP);
                //send notification to sysadmin
                if (listToken.Count > 0)
                {
                    var firebaseExtensions = new FireBaseExtentions();
                    var app = firebaseExtensions.GetFirebaseApp(path);
                    var fcm = FirebaseMessaging.GetMessaging(app);
                    Message message = new Message()
                    {
                        Notification = new Notification
                        {
                            Title = NotificationTitleHelper.NEW_REGISTRATION_CENTER_FORM_TITLE,
                            Body = NotificationBodyHelper.NEW_REGISTRATION_CENTER_FORM_BODY,
                        },
                    };
                    foreach (var token in listToken)
                    {
                        message.Token = token.DeviceToken;
                        await fcm.SendAsync(message);
                    }
                    app.Delete();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
            
            
        }
        public async Task<bool> NotificationForManagerWhenHaveNewAdoptionRegisterForm(string path, Guid centerId,Guid AdoptionRegistrationId)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var userDomain = uow.GetService<UserDomain>();
                var notificationToken = userDomain.GetManagerDeviceTokenByCenterId(centerId);
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                Message message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = NotificationTitleHelper.NEW_REGISTRATON_ADOPTION_FORM_TITLE,
                        Body = NotificationBodyHelper.NEW_REGISTRATION_ADOPTION_FORM_BODY,
                    },
                };
                if (notificationToken != null)
                {
                    message.Token = notificationToken.DeviceToken;
                    await fcm.SendAsync(message);
                }
                app.Delete();
                return true;
            }catch
            {
                return false;
            }
            
        }
        public async Task<bool> NotificationForUserWhenAdoptionFormToBeApprove(string path, Guid insertBy)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var notificationToken = FindByApplicationNameAndUserId(ApplicationNameHelper.USER_APP, insertBy);
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                Message message = new Message()
                {
                        Notification = new Notification
                        {
                            Title = NotificationTitleHelper.APPROVE_ADOPTION_FORM_TITLE,
                            Body = NotificationBodyHelper.APPROVE_ADOPTION_FORM_BODY,
                        },
                };
                if(notificationToken != null)
                {
                    message.Token = notificationToken.DeviceToken;
                    await fcm.SendAsync(message);
                }
                app.Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> NotificationForuserWhenAdoptionFormToBeFounded(string path, Guid userId)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var notificationToken = FindByApplicationNameAndUserId(ApplicationNameHelper.USER_APP, userId);
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                Message message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = NotificationTitleHelper.REJECT_ADOPTION_FORM_TITLE,
                        Body = NotificationBodyHelper.HAVE_FOUND_ADOPTION_FORM_TITLE,
                    },
                };
                if (notificationToken != null)
                {
                    message.Token = notificationToken.DeviceToken;
                    await fcm.SendAsync(message);
                }
                app.Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> NotificationForListVolunteerOfCenter(string path, List<string> topics)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                Message message = new Message()
                {
                    Notification = new Notification
                    {
                        Title = NotificationTitleHelper.NEW_RESCUE_FORM_TITLE,
                        Body = NotificationBodyHelper.NEW_RESCUE_FORM_BODY,
                    },
                };
                foreach (var topic in topics)
                {
                    message.Topic = topic;
                    await fcm.SendAsync(message);
                }
                app.Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> NofiticationForDeviceToken(string path, List<string> deviceTokens, Message message)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                foreach (var deviceToken in deviceTokens)
                {
                    message.Token = deviceToken;
                    await fcm.SendAsync(message);
                }
                app.Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> NotificationForUser(string path, Guid userId, string applicationName, Message message)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var notificationToken = FindByApplicationNameAndUserId(applicationName, userId);
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                if (notificationToken != null)
                {
                    message.Token = notificationToken.DeviceToken;
                }
                await fcm.SendAsync(message);
                app.Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> NotificationForUserWhenPickerApprovePicked(string path, Guid userId, string applicationName)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                var deviceToken = FindByApplicationNameAndUserId(ApplicationNameHelper.USER_APP, userId);
                if (deviceToken != null)
                {
                    var message = new Message();
                    message.Notification = new Notification
                    {
                        Title = NotificationTitleHelper.RESCUE_HAVE_VOLUNTEER_APPROVE_PICKED_TITLE,
                        Body = NotificationBodyHelper.RESCUE_HAVE_VOLUNTEER_APPROVE_PICKED_BODY,
                    };
                    message.Token = deviceToken.DeviceToken;
                    await fcm.SendAsync(message);
                }
                app.Delete();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public async Task<bool> NotificationForUserWhenFinderFormDelete(string path, Guid userId, string applicationName)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                var deviceToken = FindByApplicationNameAndUserId(ApplicationNameHelper.USER_APP, userId);
                if (deviceToken != null)
                {
                    var message = new Message();
                    message.Notification = new Notification
                    {
                        Title = NotificationTitleHelper.FINDER_FORM_OUT_DATE_TITLE,
                        Body = NotificationBodyHelper.FINDER_FORM_OUT_DATE_BODY,
                    };
                    message.Token = deviceToken.DeviceToken;
                    await fcm.SendAsync(message);
                }
                app.Delete();
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
