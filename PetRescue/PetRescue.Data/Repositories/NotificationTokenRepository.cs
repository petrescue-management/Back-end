using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface INotificationTokenRepository : IBaseRepository<NotificationToken, string>
    {
        NotificationToken Create(NotificationTokenCreateModel model);
        NotificationToken PrepareCreate(NotificationTokenCreateModel model);
        NotificationToken Edit(NotificationToken entity, NotificationTokenUpdateModel model);
        NotificationToken Delete(NotificationToken entity);
        NotificationToken GetNotificationTokenById(Guid notificationTokenId);
    }
    public partial class NotificationTokenRepository : BaseRepository<NotificationToken, string>, INotificationTokenRepository
    {
        public NotificationTokenRepository(DbContext context) : base(context)
        {
        }

        public NotificationToken Create(NotificationTokenCreateModel model)
        {
            var newNotificationToken = PrepareCreate(model);
            return Create(newNotificationToken).Entity;
        }

        public NotificationToken Delete(NotificationToken entity)
        {
            entity.IsActive = false;
            return Update(entity).Entity;
        }

        public NotificationToken Edit(NotificationToken entity, NotificationTokenUpdateModel model)
        {
            entity.DeviceToken = model.DeviceToken;
            return Update(entity).Entity;
        }

        public NotificationToken GetNotificationTokenById(Guid notificationTokenId)
        {
            return Get().FirstOrDefault(s => s.Id.Equals(notificationTokenId));
        }

        public NotificationToken PrepareCreate(NotificationTokenCreateModel model)
        {
            var newNotificationToken = new NotificationToken
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                DeviceToken = model.DeviceToken,
                ApplicationName = model.ApplicationName,
                UserId = model.UserId
            };
            return newNotificationToken;
        }
    }
}
