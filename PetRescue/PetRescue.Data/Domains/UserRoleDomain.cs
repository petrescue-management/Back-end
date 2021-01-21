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
    public class UserRoleDomain : BaseDomain
    {
        public UserRoleDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public UserRole RegistationRole(Guid userId, string roleName)
        {
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            var roleRepo = uow.GetService<IRoleRepository>();
            var role = roleRepo.FindRoleByName(roleName);
            if(role != null) 
                return userRoleRepo.CreateRoleForUser(userId, role.RoleId);
            return null;
        }
        public UserRole EnableRole(UserRoleUpdateModel model)
        {
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            var userRole = userRoleRepo.FindUserRoleByUserRoleUpdateModel(model);
            if(userRole != null)
            {
                var newUserRole = userRoleRepo.Edit(userRole);
                return userRoleRepo.Update(newUserRole).Entity;
            }
            return null;
        }
        public UserRole IsExisted(UserRoleUpdateModel model)
        {
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            var roleRepo = uow.GetService<IRoleRepository>();
            var role = roleRepo.FindRoleByName(model.RoleName);
            if (role != null)
            {
                return userRoleRepo.FindUserRoleByUserRoleUpdateModel(model);
            }
            return null;
        }
 
    }
}
