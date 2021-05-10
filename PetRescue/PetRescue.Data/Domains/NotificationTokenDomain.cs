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
        private readonly INotificationTokenRepository _notificationTokenRepo;
        private readonly IUserRoleRepository _userRoleRepo;
        public NotificationTokenDomain(IUnitOfWork uow, 
            INotificationTokenRepository notificationTokenRepo, 
            IUserRoleRepository userRoleRepo) : base(uow)
        {
            this._notificationTokenRepo = notificationTokenRepo;
            this._userRoleRepo = userRoleRepo;
        }
        public NotificationToken CreateNotificationToken(NotificationTokenCreateModel model)
        {
            return _notificationTokenRepo.Create(model);
        }
        public NotificationToken UpdateNotificationToken(NotificationTokenUpdateModel model)
        {
            var currentNotificationToken = _notificationTokenRepo.GetNotificationTokenById(model.Id);
            return _notificationTokenRepo.Edit(currentNotificationToken, model);
        }
        public NotificationToken DeleteNotificationToken(NotificationToken notificationToken)
        {
            return _notificationTokenRepo.Delete(notificationToken);
        }
        public NotificationToken GetById(Guid notificationId)
        {
            return _notificationTokenRepo.GetNotificationTokenById(notificationId);
        }
        public bool DeleteNotificationByUserIdAndApplicationName(Guid userId, string applicationName)
        {
            var notificationToken = _notificationTokenRepo.Get().FirstOrDefault(s => s.UserId.Equals(userId) && s.ApplicationName.Equals(applicationName));
            if(notificationToken != null)
            {
                var result = _notificationTokenRepo.Delete(notificationToken);
                if (result != null)
                {
                    return true;
                }
                return false;
            }
            return true;
        }
        public NotificationToken FindByApplicationNameAndUserId(string applicationName,Guid userId)
        {
            var notificationToken = _notificationTokenRepo.Get().FirstOrDefault(s => s.UserId == userId && s.ApplicationName.Equals(applicationName) && (bool)s.IsActived);
            return notificationToken;
        }
        public async Task<bool> NotificationForAdminWhenHaveNewCenterRegisterForm(string path)
        {
            try
            {
                var listToken = _uow.GetService<UserDomain>().GetListDeviceTokenByRoleAndApplication(RoleConstant.ADMIN, ApplicationNameHelper.SYSTEM_ADMIN_APP);
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
        public async Task<bool> NotificationForManagerWhenHaveNewAdoptionRegisterForm(string path, Guid centerId)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var notificationToken = _uow.GetService<UserDomain>().GetManagerDeviceTokenByCenterId(centerId);
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
                        Body = NotificationBodyHelper.REJECT_ADOPTION_FORM_BODY,
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
        public async Task<bool> NotificationForManager(string path, Guid centerId, Message message)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var notificationToken = _uow.GetService<UserDomain>().GetManagerDeviceTokenByCenterId(centerId);
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
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
        public async Task<bool> NotificationForUserWhenPickerApprovePicked(string path, Guid userId, string applicationName)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                var deviceToken = FindByApplicationNameAndUserId(applicationName, userId);
                if (deviceToken != null)
                {
                    var message = new Message() 
                    {
                        Notification = new Notification
                        {
                            Title = NotificationTitleHelper.RESCUE_HAVE_VOLUNTEER_APPROVE_PICKED_TITLE,
                            Body = NotificationBodyHelper.RESCUE_HAVE_VOLUNTEER_APPROVE_PICKED_BODY,
                        },
                        Token = deviceToken.DeviceToken
                    };
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
                var deviceToken = FindByApplicationNameAndUserId(applicationName, userId);
                if (deviceToken != null)
                {
                    var message = new Message()
                    {
                        Notification = new Notification
                        {
                            Title = NotificationTitleHelper.FINDER_FORM_OUT_DATE_TITLE,
                            Body = NotificationBodyHelper.FINDER_FORM_OUT_DATE_BODY,
                        },
                        Token = deviceToken.DeviceToken
                    };
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
        public async Task<bool> NotificationForUserAlertAfterAdoption(string path, Guid userId, string applicationName)
        {
            try
            {
                var firebaseExtensions = new FireBaseExtentions();
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                var deviceToken = FindByApplicationNameAndUserId(applicationName, userId);
                if (deviceToken != null)
                {
                    var message = new Message()
                    {
                        Notification = new Notification
                        {
                            Title = NotificationTitleHelper.ALERT_AFTER_ADOPTION_TITLE,
                            Body = NotificationBodyHelper.ALERT_AFTER_ADOPTION_BODY,
                        },
                        Token = deviceToken.DeviceToken
                    };
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
