using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class UserDomain : BaseDomain
    {
        public UserDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public string RegisterUser(UserCreateModel model)
        {
            var userRepo = uow.GetService<IUserRepository>();
            var newUser = userRepo.CreateUser(model);
            uow.saveChanges();
            return newUser.UserId.ToString();
        }
    }
}
