using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IRoleRepository : IBaseRepository<Role,string>
    {
        Role FindRoleByName(string rolename);
    }
    public partial class RoleRepository : BaseRepository<Role, string>,IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context)
        {
        }

        public Role FindRoleByName(string rolename)
        {
            if (rolename != null)
                return Get().FirstOrDefault(r => r.RoleName == rolename);
            return null;
        }
    }
}
