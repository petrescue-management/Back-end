using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetBreedRepository : IBaseRepository<PetBreed, string>
    {
        List<PetBreedModel> GetPetBreedsByTypeId(Guid id);

        PetBreedModel GetPetBreedById(Guid id);
    }

    public partial class PetBreedRepository : BaseRepository<PetBreed, string>, IPetBreedRepository
    {
        public PetBreedRepository(DbContext context) : base(context)
        {
        }

        public PetBreedModel GetPetBreedById(Guid id)
        {
            var breed = Get()
               .Where(b => b.PetBreedId.Equals(id))
               .Select(b => new PetBreedModel
               {
                   PetBreedId = b.PetBreedId,
                   PetBreedName = b.PetBreedName,
                   PetTypeId = b.PetTypeId
               }).FirstOrDefault();

            return breed;
        }

        public List<PetBreedModel> GetPetBreedsByTypeId(Guid id)
        {
            List<PetBreedModel> breeds = Get()
                .Where(b => b.PetTypeId.Equals(id))
                .Select(b => new PetBreedModel
                {
                    PetBreedId = b.PetBreedId,
                    PetBreedName = b.PetBreedName,
                    PetTypeId = b.PetTypeId
                }).ToList();

            return breeds;
        }
    }
}
