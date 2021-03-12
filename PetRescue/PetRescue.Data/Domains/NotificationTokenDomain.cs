using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public NotificationToken FindByApplicationNameAndUserId(Guid userId, string applicationName)
        {
            var notificationRepo = uow.GetService<INotificationTokenRepository>();
            var notificationToken = notificationRepo.Get().FirstOrDefault(s => s.UserId == userId && s.ApplicationName.Equals(applicationName) && s.IsActive);
            return notificationToken;
        }

    }
}
