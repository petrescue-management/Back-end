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
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly IRoleRepository _roleRepo;

        public UserRoleDomain(IUnitOfWork uow, 
            IUserRoleRepository userRoleRepo, 
            IRoleRepository roleRepo) : base(uow)
        {
            this._userRoleRepo = userRoleRepo;
            this._roleRepo = roleRepo;
        }
        public UserRole RegistationRole(Guid userId, string roleName, Guid insertBy)
        {
            var role = _roleRepo.FindRoleByName(roleName);
            if(role != null) 
                return _userRoleRepo.CreateRoleForUser(userId, role.RoleId, insertBy);
            return null;
        }
        public UserRole CheckRoleOfUser(UserRoleUpdateModel model) 
        {
            var role = _roleRepo.FindRoleByName(model.RoleName);
            if(role != null)
            {
                return _userRoleRepo.Get().FirstOrDefault(s => s.RoleId.Equals(role.RoleId) && s.UserId.Equals(model.UserId));
            }
            return null;

        }
        public UserRole Edit(UserRole entity,UserRoleUpdateEntityModel model)
        {
            return _userRoleRepo.Edit(entity, model);
        }
        public bool IsAdmin (string email)
        {
            var currentUser = _userRoleRepo.Get().FirstOrDefault(s => s.User.UserEmail.Equals(email) && s.Role.RoleName.Equals(RoleConstant.ADMIN));
            if(currentUser != null)
            {
                return true;
            }
            return false;
        }
    }
}
