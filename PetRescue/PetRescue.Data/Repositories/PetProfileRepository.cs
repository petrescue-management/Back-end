using Microsoft.EntityFrameworkCore;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static PetRescue.Data.ViewModels.PetProfileModel;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetProfileRepository : IBaseRepository<PetProfile, string>
    {
        PetProfileModel CreatePetProfile(CreatePetProfileModel model, Guid insertedBy, Guid centerId);
        PetProfileModel UpdatePetProfile(UpdatePetProfileModel model, Guid updatedBy);
        PetProfileModel GetPetProfileById(Guid id);
        PetProfileModel2 GetPetProfileById2(Guid id);
    }

    public partial class PetProfileRepository : BaseRepository<PetProfile, string>, IPetProfileRepository
    {
        public PetProfileRepository(DbContext context) : base(context)
        {
        }

        #region CREATE
        private PetProfile PrepareCreate(CreatePetProfileModel model, Guid insertedBy, Guid centerId)
        {
            PetProfile petProfile;

            if (model.PetDocumentId == null || model.PetDocumentId.Equals(Guid.Empty))
            {
                petProfile = new PetProfile
                {
                    RescueDocumentId = null,
                    PetName = model.PetName,
                    PetGender = model.PetGender,
                    PetAge = model.PetAge,
                    PetBreedId = model.PetBreedId,
                    PetFurColorId = model.PetFurColorId,
                    PetImgUrl = model.PetImgUrl,
                    PetStatus = model.PetStatus,
                    Description = model.Description,
                    CenterId = centerId,
                    InsertedBy = insertedBy,
                    InsertedAt = DateTime.UtcNow,
                    PetProfileId = Guid.NewGuid()
                };
            }
            else
            {
                petProfile = new PetProfile
                {
                    RescueDocumentId = model.PetDocumentId,
                    PetName = model.PetName,
                    PetGender = model.PetGender,
                    PetAge = model.PetAge,
                    PetBreedId = model.PetBreedId,
                    PetFurColorId = model.PetFurColorId,
                    PetImgUrl = model.PetImgUrl,
                    PetStatus = model.PetStatus,
                    Description = model.Description,
                    CenterId = centerId,
                    InsertedBy = insertedBy,
                    InsertedAt = DateTime.UtcNow,
                    PetProfileId = Guid.NewGuid()
                };
            }
            return petProfile;
        }
        public PetProfileModel CreatePetProfile(CreatePetProfileModel model, Guid insertedBy, Guid centerId)
        {
            var petProfile = PrepareCreate(model, insertedBy, centerId);

            Create(petProfile);

            var result = new PetProfileModel
            {
                PetDocumentId = petProfile.RescueDocumentId,
                PetName = petProfile.PetName,
                PetGender = petProfile.PetGender,
                PetAge = petProfile.PetAge,
                PetBreedId = petProfile.PetBreedId,
                PetFurColorId = petProfile.PetFurColorId,
                PetImgUrl = petProfile.PetImgUrl,
                PetStatus = petProfile.PetStatus,
                Description = petProfile.Description,
                CenterId = petProfile.CenterId,
                InsertedBy = petProfile.InsertedBy,
                InsertedAt = petProfile.InsertedAt
            };

            return result;
        }
        #endregion

        #region UPDATE
        private PetProfile PrepareUpdate(UpdatePetProfileModel model, Guid updatedBy)
        {
            var petProfile = Get().FirstOrDefault(s => s.PetProfileId.Equals(model.PetProfileId));
            if (model.Description != null)
                petProfile.Description = model.Description;
            if (model.PetName != null)
                petProfile.PetName = model.PetName;
            if (model.PetStatus != 0)
                petProfile.PetStatus = model.PetStatus;
            if (model.PetImgUrl != null)
                petProfile.PetImgUrl = model.PetImgUrl;
            if (model.PetGender != 0)
                petProfile.PetGender = model.PetGender;
            if (!model.PetBreedId.Equals(Guid.Empty))
                petProfile.PetBreedId = model.PetBreedId;
            if (!model.PetFurColorId.Equals(Guid.Empty))
                petProfile.PetFurColorId = model.PetFurColorId;
            if (model.PetAge != 0)
                petProfile.PetAge = model.PetAge;
            petProfile.UpdatedAt = DateTime.UtcNow;
            petProfile.UpdatedBy = updatedBy;
            return petProfile;
        }

        public PetProfileModel UpdatePetProfile(UpdatePetProfileModel model, Guid updatedBy)
        {
            var petProfile = PrepareUpdate(model, updatedBy);

            Update(petProfile);

            var result = new PetProfileModel
            {
                PetProfileId = petProfile.PetProfileId,
                PetDocumentId = petProfile.RescueDocumentId,
                PetName = petProfile.PetName,
                PetGender = petProfile.PetGender,
                PetAge = petProfile.PetAge,
                PetBreedId = petProfile.PetBreedId,
                PetFurColorId = petProfile.PetFurColorId,
                PetImgUrl = petProfile.PetImgUrl,
                PetStatus = petProfile.PetStatus,
                Description = petProfile.Description,
                CenterId = petProfile.CenterId,
                InsertedBy = petProfile.InsertedBy,
                InsertedAt = petProfile.InsertedAt,
                UpdatedAt = petProfile.UpdatedAt,
                UpdatedBy = petProfile.UpdatedBy,
                
            };

            return result;
        }
        #endregion

        #region GET BY ID
        public PetProfileModel GetPetProfileById(Guid id)
        {
            var result = Get()
               .Where(p => p.PetProfileId.Equals(id))
               .Include(p => p.PetBreed)
               .Include(p => p.PetFurColor)
               .Select(p => new PetProfileModel
               {
                   PetDocumentId = p.RescueDocumentId,
                   CenterId = p.CenterId,
                   Description = p.Description,
                   PetAge = p.PetAge,
                   PetBreedId = p.PetBreedId,
                   PetBreedName = p.PetBreed.PetBreedName,
                   PetFurColorId = p.PetFurColorId,
                   PetFurColorName = p.PetFurColor.PetFurColorName,
                   PetGender = p.PetGender,
                   PetName = p.PetName,
                   PetStatus = p.PetStatus,
                   PetImgUrl = p.PetImgUrl,
                   PetProfileId = p.PetProfileId
               }).FirstOrDefault();
            return result;
        }

        public PetProfileModel2 GetPetProfileById2(Guid id)
        {
            var result = Get()
               .Where(p => p.PetProfileId.Equals(id))
               .Include(p => p.PetBreed)
               .Include(p => p.PetFurColor)
               .Select(p => new PetProfileModel2
               {
                   PetDocumentId = p.RescueDocumentId,
                   CenterId = p.CenterId,
                   Description = p.Description,
                   PetAge = p.PetAge,
                   PetBreedId = p.PetBreedId,
                   PetBreedName = p.PetBreed.PetBreedName,
                   PetFurColorId = p.PetFurColorId,
                   PetFurColorName = p.PetFurColor.PetFurColorName,
                   PetGender = p.PetGender,
                   PetName = p.PetName,
                   PetStatus = p.PetStatus,
                   PetImgUrl = p.PetImgUrl,
                   PetProfileId = p.PetProfileId,
                   CenterAddress = p.Center.Address,
                   CenterName = p.Center.CenterName,
                   InsertedAt = p.InsertedAt,
                   InsertedBy = p.InsertedBy,
               }).FirstOrDefault();
            return result;
        }
        #endregion
    }

}
