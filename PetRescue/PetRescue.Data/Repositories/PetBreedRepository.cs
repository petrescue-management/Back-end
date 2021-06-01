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
        PetBreed Create(PetBreedCreateModel model);
        PetBreed PrepareCreate(PetBreedCreateModel model);
        PetBreed Edit(PetBreedUpdateModel model, PetBreed entity);

        PetBreed Remove(Guid petFurColorId);
    }

    public partial class PetBreedRepository : BaseRepository<PetBreed, string>, IPetBreedRepository
    {
        public PetBreedRepository(DbContext context) : base(context)
        {
        }

        public PetBreed Create(PetBreedCreateModel model)
        {
            var newPetBreed = PrepareCreate(model);
            Create(newPetBreed);
            return newPetBreed;
        }

        public PetBreed Edit(PetBreedUpdateModel model, PetBreed entity)
        {
            entity.PetBreedName = model.PetBreedName;
            return Update(entity).Entity;
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

        public PetBreed PrepareCreate(PetBreedCreateModel model)
        {
            var newPetBreed = new PetBreed
            {
                PetBreedId = Guid.NewGuid(),
                PetBreedName = model.PetBreedName,
                PetTypeId = model.PetTypeId
            };
            return newPetBreed;
        }

        public PetBreed Remove(Guid petFurColorId)
        {
            throw new NotImplementedException();
        }
    }
}
