using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IRoleRepository : IBaseRepository<Role,string>
    {
    }
    public partial class RoleRepository : BaseRepository<Role, string>,IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context)
        {
        }
    }
}
