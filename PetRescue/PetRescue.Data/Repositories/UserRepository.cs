using Microsoft.EntityFrameworkCore;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IUserRepository : IBaseRepository<User, string>
    {
        User CreateUser(UserCreateByAppModel model);
        User PrepareCreate(UserCreateByAppModel model);
        User FindById(string email = null, string id = null);
        User CreateUserByModel(UserCreateModel model);
        User UpdateUserModel(User entity, UserUpdateModel model);
        UserModel GetUserById(Guid id);
    }
    public partial class UserRepository : BaseRepository<User, string>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public User CreateUser(UserCreateByAppModel model)
        {
            var user = PrepareCreate(model);
            Create(user);
            return user;
        }

        public User FindById(string email = null, string id =  null)
        {
            if(email != null)
            {
                return Get().FirstOrDefault(e => e.UserEmail == email);
            }
            if(id != null)
            {
                return Get().FirstOrDefault(e => e.UserId.ToString() == id);
            }
            return null;
        }

        public User PrepareCreate(UserCreateByAppModel model)
        {
            User user = new User
            {
                UserEmail = model.Email,
                Password = model.Password,
                UserId = Guid.NewGuid(),
                UserStatus = UserStatus.DEFAULT,
            };
            return user;  
        }
        public User CreateUserByModel(UserCreateModel model)
        {
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                UserEmail = model.Email,
                CenterId = model.CenterId
            };
            return Create(newUser).Entity;
        }
        public User UpdateUserModel(User entity, UserUpdateModel model)
        {
            entity.CenterId = model.CenterId;
            //entity.UserStatus = model.UserStatus;
            return Update(entity).Entity;
        }
        public UserModel GetUserById(Guid id)
        {
            return Get().Where(u => u.UserId.Equals(id))
                .Include(u => u.UserProfile)
                .Select(u => new UserModel { 
                UserId = u.UserId,
                UserEmail = u.UserEmail,
                FirstName = u.UserProfile.FirstName,
                LastName = u.UserProfile.LastName,
                Dob = u.UserProfile.Dob,
                Gender = u.UserProfile.Gender,
                Phone = u.UserProfile.Phone,
                ImageUrl = u.UserProfile.UserImgUrl
                }).FirstOrDefault();
        }
    }
}
