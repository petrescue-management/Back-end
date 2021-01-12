using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IUserRepository : IBaseRepository<User, string>
    {
        User CreateUser(UserCreateModel model);
        User PrepareCreate(UserCreateModel model);

        User FindById(string username, string id);

    }
    public partial class UserRepository : BaseRepository<User, string>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public User CreateUser(UserCreateModel model)
        {
            var user = PrepareCreate(model);
            Create(user);
            SaveChanges();
            return user;
        }

        public User FindById(string username, string id)
        {
            throw new NotImplementedException();
        }

        public User PrepareCreate(UserCreateModel model)
        {
            User user = new User
            {
                
            };
            return user;  
        }
    }
}
