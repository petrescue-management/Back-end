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
        User CreateUser(string email);
        User PrepareCreate(string email);

        User FindById(string email = null, string id = null);

        User Edit(User entity, Guid centerId);

        User CreateUserByModel(UserCreateModel model);

    }
    public partial class UserRepository : BaseRepository<User, string>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public User CreateUser(string email)
        {
            var user = PrepareCreate(email);
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

        public User PrepareCreate(string email)
        {
            User user = new User
            {
                UserEmail = email,
                IsBelongToCenter = false,

            };
            return user;  
        }

        public User Edit(User entity, Guid centerId)
        {
            entity.CenterId = centerId;
            entity.IsBelongToCenter = true;
            return entity;
        }

        public User CreateUserByModel(UserCreateModel model)
        {
            var newUser = new User
            {
                UserId = Guid.NewGuid(),
                CenterId = model.CenterId,
                IsBelongToCenter = model.IsBelongToCenter,
                UserEmail = model.Email
            };
            Create(newUser);
            return newUser;
        }
    }
}
