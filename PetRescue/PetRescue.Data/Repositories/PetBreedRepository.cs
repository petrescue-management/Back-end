using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetBreedRepository : IBaseRepository<PetBreed, string>
    {
        
    }

    public partial class PetBreedRepository : BaseRepository<PetBreed, string>, IPetBreedRepository
    {
        public PetBreedRepository(DbContext context) : base(context)
        {
        }
    }
}
