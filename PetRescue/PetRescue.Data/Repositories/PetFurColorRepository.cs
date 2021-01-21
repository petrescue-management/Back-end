using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetFurColorRepository : IBaseRepository<PetFurColor, string>
    {

    }

    public partial class PetFurColorRepository : BaseRepository<PetFurColor, string>, IPetFurColorRepository
    {
        public PetFurColorRepository(DbContext context) : base(context)
        {
        }
    }
}
