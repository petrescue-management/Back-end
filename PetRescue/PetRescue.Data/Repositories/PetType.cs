using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetTypeRepository : IBaseRepository<PetType, string>
    {

    }

    public partial class PetTypeRepository : BaseRepository<PetType, string>, IPetTypeRepository
    {
        public PetTypeRepository(DbContext context) : base(context)
        {
        }
    }
}
