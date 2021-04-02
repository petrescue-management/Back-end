using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetTrackingRepository : IBaseRepository<PetTracking, string>
    {
        PetTracking Create(PetTrackingCreateModel model, Guid insertBy);
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
                PetDocumentId = model.PetDocumentId,
                PetTrackingImgUrl = model.ImageUrl,
                Weight = (int)model.Weight,
                PetTrackingId = Guid.NewGuid()
            };
            return track;
        }
    }
}
