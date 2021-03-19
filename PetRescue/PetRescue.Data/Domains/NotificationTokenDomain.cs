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
        public NotificationToken FindByApplicationNameAndUserId(string applicationName,Guid userId)
        {
            var notificationRepo = uow.GetService<INotificationTokenRepository>();
            var notificationToken = notificationRepo.Get().FirstOrDefault(s => s.UserId == userId && s.ApplicationName.Equals(applicationName) && s.IsActive);
            return notificationToken;
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
        public async Task<bool> NotificationForManagerWhenHaveNewAdoptionRegisterForm(string path, Guid centerId)
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
                message.Token = notificationToken.DeviceToken;
                await fcm.SendAsync(message);
                app.Delete();
                return true;
            }catch
            {
                return false;
            }
            
        }
        public async Task<bool> NotificationForUserWhenAdoptionFormToBeChangeStatus(string path, Guid insertBy, int status)
        {
            try
            {
                var notificationTokenDomain = uow.GetService<NotificationTokenDomain>();
                var firebaseExtensions = new FireBaseExtentions();
                var notificationToken = notificationTokenDomain.FindByApplicationNameAndUserId(ApplicationNameHelper.USER_APP, insertBy);
                var app = firebaseExtensions.GetFirebaseApp(path);
                var fcm = FirebaseMessaging.GetMessaging(app);
                if (status == AdoptionRegisterFormStatusConst.APPROVED)
                {
                    Message message = new Message()
                    {
                        Notification = new Notification
                        {
                            Title = NotificationTitleHelper.APPROVE_ADOPTION_FORM_TITLE,
                            Body = NotificationBodyHelper.APPROVE_ADOPTION_FORM_BODY,
                        },
                    };
                    message.Token = notificationToken.DeviceToken;
                    await fcm.SendAsync(message);
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
    }
}
