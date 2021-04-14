using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IAdoptionReportTrackingRepository: IBaseRepository<AdoptionReportTracking, string>
    {
        AdoptionReportTracking Create(AdoptionReportTrackingCreateModel model, Guid insertedBy);
        AdoptionReportTracking Edit(AdoptionReportTrackingUpdateModel model, Guid insertedBy);
    }
    public class AdoptionReportTrackingRepository : BaseRepository<AdoptionReportTracking, string>, IAdoptionReportTrackingRepository
    {
        public AdoptionReportTrackingRepository(DbContext context) : base(context)
        {
        }

        public AdoptionReportTracking Create(AdoptionReportTrackingCreateModel model, Guid insertedBy)
        {
            var result = PrepareCreate(model, insertedBy);
            return Create(result).Entity;
        }

        public AdoptionReportTracking Edit(AdoptionReportTrackingUpdateModel model, Guid insertedBy)
        {
            var result = PrepareUpdate(model, insertedBy);
            return Update(result).Entity;
        }
        private AdoptionReportTracking PrepareUpdate(AdoptionReportTrackingUpdateModel model, Guid insertedBy)
        {
            var adoptionReportTracking = Get().FirstOrDefault(s => s.AdoptionReportTrackingId.Equals(model.AdoptionReportTrackingId));
            if(model.AdoptionReportImage != null)
            {
                adoptionReportTracking.AdoptionReportTrackingImgUrl = model.AdoptionReportImage,
            }
            if(model.Description != null)
            {
                adoptionReportTracking.Description = model.Description;
            }
            return adoptionReportTracking;
        }
        private AdoptionReportTracking PrepareCreate(AdoptionReportTrackingCreateModel model, Guid insertedBy)
        {
            var result = new AdoptionReportTracking
            {
                AdoptionReportTrackingId = Guid.NewGuid(),
                AdoptionReportTrackingImgUrl = model.AdoptionReportImage,
                Description = model.Description,
                InsertedAt = DateTime.UtcNow,
                InsertedBy = insertedBy,
                PetProfileId = model.PetProfileId
            };
            return result;
        }
    }
}
