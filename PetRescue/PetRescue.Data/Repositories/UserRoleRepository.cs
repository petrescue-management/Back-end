using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PetRescue.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetRescue.Data.Repositories
{
    public partial interface IUserRoleRepository: IBaseRepository<UserRole, string>
    {
        UserRole CreateRoleForUser(Guid userId, Guid roleId);
        UserRole PrepareCreateRole(Guid userId, Guid roleId);
    }
    public partial class UserRoleRepository : BaseRepository<UserRole,string>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext context) : base(context)
        {
        }
        public UserRole CreateRoleForUser(Guid userId, Guid roleId)
        {
            var userRole = PrepareCreateRole(userId, roleId);
            Create(userRole);
            SaveChanges();
            return userRole;
        }

        public UserRole PrepareCreateRole(Guid userId, Guid roleId)
        {
            
                UserRole userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId,
                    InsertedBy = null,
                    InsertedAt = DateTime.Now,
                    UpdateAt = null,
                    UpdateBy = null
                };
                return userRole;
            
        }
    }

}
