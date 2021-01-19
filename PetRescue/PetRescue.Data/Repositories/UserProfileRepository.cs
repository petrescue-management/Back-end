using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IUserProfileRepository : IBaseRepository<UserProfile, string>
    {
    }
    public partial class UserProfileRepository : BaseRepository<UserProfile, string>, IUserProfileRepository
    {
        public UserProfileRepository(DbContext context) : base(context)
        {
        }
    }
}
