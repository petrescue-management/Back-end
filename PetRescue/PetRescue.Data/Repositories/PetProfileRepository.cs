using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetProfileRepository : IBaseRepository<PetProfile, string>
    {
        PetProfile Create(PetDetailModel model);
        PetProfile Edit(PetDetailModel model, PetProfile entity);
        PetProfile PrepareCreate(PetDetailModel model);
    }

    public partial class PetProfileRepository : BaseRepository<PetProfile, string>, IPetProfileRepository
    {
        public PetProfileRepository(DbContext context) : base(context)
        {
        }

        public PetProfile Create(PetDetailModel model)
        {
            var newPetProfile = PrepareCreate(model);
            return Create(newPetProfile).Entity;
        }

        public PetProfile Edit(PetDetailModel model, PetProfile entity)
        {
            entity.PetAge = model.PetAge;
            entity.PetBreedId = model.PetBreedId;
            entity.PetName = model.PetName;
            entity.PetGender = model.PetGender;
            entity.PetFurColorId = model.PetFurColorId;
            entity.Weight = model.Weight;
            entity.ImageUrl = model.ImageUrl;
            entity.IsSterilized = model.IsSterilized;
            entity.IsVaccinated = model.IsVaccinated;
            entity.Description = model.Description;
            return Update(entity).Entity;
        }

        public PetProfile PrepareCreate(PetDetailModel model)
        {
            var newPetProfile = new PetProfile
            {
                PetId = model.PetId,
                Description = model.Description,
                PetAge = model.PetAge,
                PetBreedId = model.PetBreedId,
                IsSterilized = model.IsSterilized,
                IsVaccinated = model.IsVaccinated,
                PetFurColorId = model.PetFurColorId,
                PetGender = model.PetGender,
                PetName = model.PetName,
                Weight = model.Weight,
                ImageUrl = model.ImageUrl
            };
            return newPetProfile;
        }
    }
}
