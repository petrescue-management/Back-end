using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetRepository : IBaseRepository<Pet, string>
    {
        List<PetBreed> GetPetBreedsByTypeId(Guid id, DbSet<PetBreed> model);

        PetBreed GetPetBreedById(Guid id, DbSet<PetBreed> service);
    }

    public partial class PetRepository : BaseRepository<Pet, string>, IPetRepository
    {
        public PetRepository(DbContext context) : base(context)
        {
        }

        public PetBreed GetPetBreedById(Guid id, DbSet<PetBreed> service)
        {
            var breed = service
               .Where(b => b.PetBreedId.Equals(id))
               .Select(b => new PetBreed
               {
                   PetBreedId = b.PetBreedId,
                   PetBreedName = b.PetBreedName,
                   PetTypeId = b.PetTypeId
               }).FirstOrDefault();

            return breed;
        }

        public List<PetBreed> GetPetBreedsByTypeId(Guid id, DbSet<PetBreed> service)
        {
            List<PetBreed> breeds = service
                .Where(b => b.PetTypeId.Equals(id))
                .Select(b => new PetBreed
            {
                PetBreedId = b.PetBreedId,
                PetBreedName = b.PetBreedName,
                PetTypeId = b.PetTypeId
            }).ToList();

            return breeds;
        }
    }
}
