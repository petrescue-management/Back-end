using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetTrackingRepository : IBaseRepository<PetTracking, string>
    {
        PetTracking Create(PetTrackingCreateModel model, Guid insertBy);

        PetTracking CreatePetTrackingByUser(CreatePetTrackingByUserModel model, Guid insertedBy);
    }
    public partial class PetTrackingRepository : BaseRepository<PetTracking, string>, IPetTrackingRepository
    {
        public PetTrackingRepository(DbContext context) : base(context)
        {
        }

        public PetTracking Create(PetTrackingCreateModel model, Guid insertBy)
        {
            var track = PrepareCreate(model, insertBy);
            return Create(track).Entity;
        }

        private PetTracking PrepareCreate(PetTrackingCreateModel model,Guid insertBy)
        {
            var track = new PetTracking
            {
                Description = model.Description,
                InsertedAt = DateTime.UtcNow,
                InsertedBy = insertBy,
                IsSterilized = model.isSterilized,
                IsVaccinated = model.isVaccinated,
                PetProfileId = model.PetProfileId,
                PetTrackingImgUrl = model.ImageUrl,
                Weight = model.Weight,
                PetTrackingId = Guid.NewGuid()
            };
            return track;
        }

        private PetTracking PrepareCreateByUser(CreatePetTrackingByUserModel model, Guid insertedBy)
        {
            var tracking = Get().OrderBy(t => t.InsertedAt).Select(t => new PetTracking
            {
                PetTrackingId = Guid.NewGuid(),
                Description = model.Description,
                InsertedAt = DateTime.UtcNow,
                InsertedBy = insertedBy,
                IsSterilized = t.IsSterilized,
                IsVaccinated = t.IsVaccinated,
                PetProfileId = model.PetProfileId,
                PetTrackingImgUrl = model.ImageUrl,
                Weight = t.Weight,
                
            }).FirstOrDefault();

            return tracking;
        }

        public PetTracking CreatePetTrackingByUser(CreatePetTrackingByUserModel model, Guid insertedBy)
        {
            var track = PrepareCreateByUser(model, insertedBy);
            Create(track);
            return track;
        }
    }
}
