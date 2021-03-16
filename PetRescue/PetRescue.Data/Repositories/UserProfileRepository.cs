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

        UserProfile PrepareCraete(UserProfileUpdateModel model);

        UserProfile Edit(UserProfile entity,UserProfileUpdateModel model);
        
    }
    public partial class UserProfileRepository : BaseRepository<UserProfile, string>, IUserProfileRepository
    {
        public UserProfileRepository(DbContext context) : base(context)
        {
        }

        public UserProfile Create(UserProfileUpdateModel model)
        {
            UserProfile userProfile = PrepareCraete(model);
            Create(userProfile);
            return userProfile;
        }

        public UserProfile Edit(UserProfile entity,UserProfileUpdateModel model)
        {
            entity.LastName = model.LastName;
            entity.FirstName = model.FirstName;
            entity.Address = model.Address;
            entity.Dob = model.DoB;
            entity.Gender = model.Gender;
            entity.Phone = model.Phone;
            entity.ImageUrl = model.ImgUrl;
            return entity;
        }

        public UserProfile FindById(Guid userId)
        {
            if (userId != null)
            {
                return Get().FirstOrDefault(u => u.UserId == userId);
            }
            return null;
        }

        public UserProfile PrepareCraete(UserProfileUpdateModel model)
        {
            var newUserProfile = new UserProfile
            {
                UserId = model.UserId,
                LastName = model.LastName,
                FirstName = model.FirstName,
                Address = model.Address,
                Dob = model.DoB,
                Phone = model.Phone,
                Gender = model.Gender,
                ImageUrl = model.ImgUrl
            };
            return newUserProfile;
        }
    }
}
