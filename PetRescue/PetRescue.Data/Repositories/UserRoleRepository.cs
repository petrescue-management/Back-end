using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetRescue.Data.Repositories
{
    public partial interface IUserRoleRepository: IBaseRepository<UserRole, string>
    {
        UserRole CreateRoleForUser(Guid userId, Guid roleId, Guid insertBy);
        UserRole PrepareCreateRole(Guid userId, Guid roleId, Guid insertBy);
        UserRole Edit(UserRole entity, UserRoleUpdateEntityModel model);
    }
    public partial class UserRoleRepository : BaseRepository<UserRole,string>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext context) : base(context)
        {
        }
        public UserRole CreateRoleForUser(Guid userId, Guid roleId, Guid insertBy)
        {
            var userRole = PrepareCreateRole(userId, roleId, insertBy);
            Create(userRole);
            return userRole;
        }

        public UserRole Edit(UserRole entity,  UserRoleUpdateEntityModel model)
        {
            entity.IsActived = model.IsActived;
            return Update(entity).Entity;
        }
        public UserRole PrepareCreateRole(Guid userId, Guid roleId, Guid insertBy)
        {

            UserRole userRole = new UserRole
            {
                UserId = userId,
                RoleId = roleId,
                InsertedAt = DateTime.Now,
                UpdatedAt = null,
                IsActived = true
            };
            return userRole; 
        }
       
    }

}
