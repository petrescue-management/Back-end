using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
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
            var roleId = roleRepo.Get().FirstOrDefault(r => r.RoleName == roleName).RoleId;
            var newUserRole = userRoleRepo.CreateRoleForUser(userId, roleId);
            return newUserRole;
        }
 
    }
}
