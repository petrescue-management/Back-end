﻿using Microsoft.EntityFrameworkCore;
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
        Pet Create(PetCreateModel model, Guid insertBy, Guid centerId);
        Pet Edit(Pet entity, PetDetailModel model, Guid updateBy);
        Pet Delete (Pet entity);
        Pet PrepareCreate(PetCreateModel model, Guid insertBy, Guid centerId);

        PetModel GetPetById(Guid id);
    }

    public partial class PetRepository : BaseRepository<Pet, string>, IPetRepository
    {
        public PetRepository(DbContext context) : base(context)
        {
        }

        public Pet Create(PetCreateModel model, Guid insertBy, Guid centerId)
        {

            var newPet = PrepareCreate(model,insertBy, centerId);
            Create(newPet);
            return newPet;
        }

        public Pet Delete(Pet entity)
        {
            throw new NotImplementedException();
        }

        public Pet Edit(Pet entity, PetDetailModel model, Guid updateBy)
        {
            if(model.Description != null)
                entity.PetNavigation.Description = model.Description;
            if (model.PetAge != null)
                entity.PetNavigation.PetAge = model.PetAge;
            if (model.PetBreedId != null)
                entity.PetNavigation.PetBreedId = model.PetBreedId;
            if (model.PetFurColorId != null)
                entity.PetNavigation.PetFurColorId = model.PetFurColorId;
            if (ValidationExtensions.IsNotNull(model.PetGender))
                entity.PetNavigation.PetGender = model.PetGender;
            if (ValidationExtensions.IsNotNullOrEmpty(model.PetName))
                entity.PetNavigation.PetName = model.PetName;
            entity.UpdatedBy = updateBy;
            entity.UpdatedAt = DateTime.UtcNow;
            return Update(entity).Entity;
        }

        public Pet PrepareCreate(PetCreateModel model,Guid insertBy, Guid centerId)
        {
            var newPet = new Pet
            {
                PetId = Guid.NewGuid(),
                CenterId = centerId,
                PetStatus = model.PetStatus,
                InsertedAt = DateTime.UtcNow,
                InsertedBy = insertBy,
                UpdatedAt = null,
                UpdatedBy = null,
            };
            return newPet;
        }

       public PetModel GetPetById(Guid id)
        {
            var result = Get()
               .Where(p => p.PetId.Equals(id))
               .Include(p => p.PetNavigation)
               .ThenInclude(p => p.PetBreed)
               .Include(p => p.PetNavigation.PetFurColor)
               .Select(p => new PetModel
               {
                   PetId = p.PetId,
                   CenterId = p.CenterId,
                   Description = p.PetNavigation.Description,
                   PetAge = p.PetNavigation.PetAge,
                   PetBreedId = p.PetNavigation.PetBreedId,
                   PetBreedName = p.PetNavigation.PetBreed.PetBreedName,
                   PetFurColorId = p.PetNavigation.PetFurColorId,
                   PetFurColorName = p.PetNavigation.PetFurColor.PetFurColorName,
                   PetGender = p.PetNavigation.PetGender,
                   PetName = p.PetNavigation.PetName,
                   PetStatus = p.PetStatus,
                   ImgUrl = p.PetNavigation.ImageUrl
               }).FirstOrDefault();
            return result;
        }
    }
}
