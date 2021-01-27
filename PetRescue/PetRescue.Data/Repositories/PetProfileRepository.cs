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
            Create(newPetProfile);
            return newPetProfile;
        }

        public PetProfile Edit(PetDetailModel model, PetProfile entity)
        {
            throw new NotImplementedException();
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
                Weight = model.Weight
            };
            return newPetProfile;
        }
    }
}
