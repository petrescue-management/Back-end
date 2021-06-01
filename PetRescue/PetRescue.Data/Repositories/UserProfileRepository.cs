using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IUserProfileRepository : IBaseRepository<UserProfile, string>
    {
        UserProfile FindById(Guid userId);

        UserProfile Create(UserProfileUpdateModel model);

        UserProfile PrepareCreate(UserProfileUpdateModel model);

        UserProfile Edit(UserProfile entity,UserProfileUpdateModel model);
        
    }
    public partial class UserProfileRepository : BaseRepository<UserProfile, string>, IUserProfileRepository
    {
        public UserProfileRepository(DbContext context) : base(context)
        {
        }

        public UserProfile Create(UserProfileUpdateModel model)
        {
            UserProfile userProfile = PrepareCreate(model);
            return Create(userProfile).Entity;
        }

        public UserProfile Edit(UserProfile entity,UserProfileUpdateModel model)
        {
            entity.LastName = model.LastName;
            entity.FirstName = model.FirstName;
            entity.Dob = model.DoB;
            entity.Gender = model.Gender;
            entity.Phone = model.Phone;
            entity.UserImgUrl = model.ImgUrl;
            entity.UpdatedAt = DateTime.UtcNow;
            return Update(entity).Entity;
        }
        public UserProfile FindById(Guid userId)
        {
            if (userId != null)
            {
                return Get().FirstOrDefault(u => u.UserId == userId);
            }
            return null;
        }
        public UserProfile PrepareCreate(UserProfileUpdateModel model)
        {
            var newUserProfile = new UserProfile
            {
                UserId = model.UserId,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Dob = model.DoB,
                Phone = model.Phone,
                Gender = 3,
                UserImgUrl = model.ImgUrl,
                InsertedAt = DateTime.UtcNow
            };
            return newUserProfile;
        }
    }
}
