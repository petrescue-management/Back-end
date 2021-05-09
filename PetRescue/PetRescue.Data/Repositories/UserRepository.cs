using Microsoft.EntityFrameworkCore;
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
            };
            return user;  
        }
        public User CreateUserByModel(UserCreateModel model)
        {
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                UserEmail = model.Email
            };
            return Create(newUser).Entity;
        }
        public User UpdateUserModel(User entity, UserUpdateModel model)
        {
            //entity.CenterId = model.CenterId;
            entity.UserStatus = model.UserStatus;
            return Update(entity).Entity;
        }
        public UserModel GetUserById(Guid id)
        {
            return Get().Where(u => u.UserId.Equals(id))
                .Include(u => u.UserNavigation)
                .Select(u => new UserModel { 
                UserId = u.UserId,
                UserEmail = u.UserEmail,
                FirstName = u.UserNavigation.FirstName,
                LastName = u.UserNavigation.LastName,
                Dob = u.UserNavigation.Dob,
                Gender = u.UserNavigation.Gender,
                Phone = u.UserNavigation.Phone,
                ImageUrl = u.UserNavigation.UserImgUrl
                }).FirstOrDefault();
        }
    }
}
