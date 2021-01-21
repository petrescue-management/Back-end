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
        UserRole CreateRoleForUser(Guid userId, Guid roleId);
        UserRole PrepareCreateRole(Guid userId, Guid roleId);

        UserRole FindUserRoleByUserRoleUpdateModel(UserRoleUpdateModel model);

        UserRole Edit(UserRole userRole);
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
            
            return userRole;
        }

        public UserRole Edit(UserRole userRole)
        {
            //userRole.UpdateAt = DateTime.Now;
            //userRole.UpdateBy = userRole.UpdateBy;
            //userRole.IsActived = userRole.IsActived == true ? false : true;
            return userRole;
        }

        public UserRole FindUserRoleByUserRoleUpdateModel(UserRoleUpdateModel model)
        {
            if(model.CenterId != null && model.RoleName != null && model.UserId != null)
            {
                return Get().FirstOrDefault(u => u.Role.RoleName == model.RoleName && u.UserId == model.UserId && u.User.CenterId == model.CenterId);
            }
            return null;
            
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
