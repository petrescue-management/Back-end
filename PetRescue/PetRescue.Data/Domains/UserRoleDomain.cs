using PetRescue.Data.ConstantHelper;
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
        public UserRole RegistationRole(Guid userId, string roleName, Guid insertBy)
        {
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            var roleRepo = uow.GetService<IRoleRepository>();
            var role = roleRepo.FindRoleByName(roleName);
            if(role != null) 
                return userRoleRepo.CreateRoleForUser(userId, role.RoleId, insertBy);
            return null;
        }
        public UserRole CheckRoleOfUser(UserRoleUpdateModel model) 
        {
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            var roleRepo = uow.GetService<IRoleRepository>();
            var role = roleRepo.FindRoleByName(model.RoleName);
            if(role != null)
            {
                return userRoleRepo.Get().FirstOrDefault(s => s.RoleId.Equals(role.RoleId) && s.UserId.Equals(model.UserId));
            }
            return null;

        }
        public UserRole Edit(UserRole entity,UserRoleUpdateEntityModel model)
        {
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            return userRoleRepo.Edit(entity, model);
        }
        public bool IsAdmin (string email)
        {
            var userRoleRepo = uow.GetService<IUserRoleRepository>();
            var currentUser = userRoleRepo.Get().FirstOrDefault(s => s.User.UserEmail.Equals(email) && s.Role.RoleName.Equals(RoleConstant.ADMIN));
            if(currentUser != null)
            {
                return true;
            }
            return false;

        }
 
    }
}
