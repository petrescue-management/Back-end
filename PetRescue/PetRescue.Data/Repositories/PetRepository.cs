using Microsoft.EntityFrameworkCore;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Extensions;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetRepository : IBaseRepository<Pet, string>
    {
        Pet Create(PetCreateModel model);
        Pet Edit(Pet entity, PetDetailModel model);
        Pet Delete (Pet entity);
        Pet PrepareCreate(PetCreateModel model);
    }

    public partial class PetRepository : BaseRepository<Pet, string>, IPetRepository
    {
        public PetRepository(DbContext context) : base(context)
        {
        }

        public Pet Create(PetCreateModel model)
        {

            var newPet = PrepareCreate(model);
            Create(newPet);
            return newPet;
        }

        public Pet Delete(Pet entity)
        {
            throw new NotImplementedException();
        }

        public Pet Edit(Pet entity, PetDetailModel model)
        {
           if(model.Description != null)
                entity.PetNavigation.Description = model.Description;
            if (ValidationExtensions.IsNotNull(model.IsSterilized))
                entity.PetNavigation.IsSterilized = model.IsSterilized;
            if (ValidationExtensions.IsNotNull(model.IsVaccinated))
                entity.PetNavigation.IsVaccinated = model.IsVaccinated;
            if (model.PetAge != null)
                entity.PetNavigation.PetAge = model.PetAge;
            if (model.PetBreedId != null)
                entity.PetNavigation.PetBreedId = model.PetBreedId;
            if (model.PetFurColorId != null)
                entity.PetNavigation.PetFurColorId = model.PetFurColorId;
            if (ValidationExtensions.IsNotNull(model.PetGender))
                entity.PetNavigation.PetGender = model.PetGender;
            if (model.PetName != null)
                entity.PetNavigation.PetName = model.PetName;
            if (ValidationExtensions.IsNotNull(model.Weight))
                entity.PetNavigation.Weight = model.Weight;
            return entity;
        }

        public Pet PrepareCreate(PetCreateModel model)
        {
            var newPet = new Pet
            {
                PetId = Guid.NewGuid(),
                CenterId = model.CenterId,
                PetStatus = model.PetStatus,
                InsertedAt = DateTime.UtcNow,
                InsertedBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                UpdatedAt = null,
                UpdatedBy = null,
            };
            return newPet;
        }

       
    }
}
