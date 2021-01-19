﻿using Microsoft.EntityFrameworkCore;
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
            SaveChanges();
            return user;
        }

        public User FindById(string email = null, string id =  null)
        {
            if(email != null)
            {
                return Get().FirstOrDefault(e => e.UserEmail == email);
            }
            else if(id != null)
            {
                return Get().FirstOrDefault(e => e.UserId.ToString() == id);
            }
            return null;
        }

        public User PrepareCreate(string email)
        {
            User user = new User
            {
                UserEmail = email
            };
            return user;  
        }

    }
}
