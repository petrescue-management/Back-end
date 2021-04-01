using Microsoft.EntityFrameworkCore;
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

            if (model.PetDocumentId == null)
            {
                petProfile = new PetProfile
                {
                    PetDocumentId = Guid.NewGuid(),
                    PetName = model.PetName,
                    PetGender = model.PetGender,
                    PetAge = model.PetAge,
                    PetBreedId = model.PetBreedId,
                    PetFurColorId = model.PetFurColorId,
                    PetImgUrl = model.PetImgUrl,
                    PetStatus = model.PetStatus,
                    PetProfileDescription = model.PetProfileDescription,
                    CenterId = centerId,
                    InsertedBy = insertedBy,
                    InsertedAt = DateTime.UtcNow
                };
            }
            else
            {
                petProfile = new PetProfile
                {
                    PetDocumentId = model.PetDocumentId,
                    PetName = model.PetName,
                    PetGender = model.PetGender,
                    PetAge = model.PetAge,
                    PetBreedId = model.PetBreedId,
                    PetFurColorId = model.PetFurColorId,
                    PetImgUrl = model.PetImgUrl,
                    PetStatus = model.PetStatus,
                    PetProfileDescription = model.PetProfileDescription,
                    CenterId = centerId,
                    InsertedBy = insertedBy,
                    InsertedAt = DateTime.UtcNow
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
                PetDocumentId = petProfile.PetDocumentId,
                PetName = petProfile.PetName,
                PetGender = petProfile.PetGender,
                PetAge = petProfile.PetAge,
                PetBreedId = petProfile.PetBreedId,
                PetFurColorId = petProfile.PetFurColorId,
                PetImgUrl = petProfile.PetImgUrl,
                PetStatus = petProfile.PetStatus,
                PetProfileDescription = petProfile.PetProfileDescription,
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
            var petProfile = Get()
                    .Where(p => p.PetDocumentId.Equals(model.PetDocumentId))
                    .Select(p => new PetProfile
                    {
                        PetDocumentId = model.PetDocumentId,
                        PetName = model.PetName,
                        PetGender = model.PetGender,
                        PetAge = model.PetAge,
                        PetBreedId = model.PetBreedId,
                        PetFurColorId = model.PetFurColorId,
                        PetImgUrl = model.PetImgUrl,
                        PetStatus = model.PetStatus,
                        PetProfileDescription = model.PetProfileDescription,
                        CenterId = p.InsertedBy,
                        InsertedBy = p.InsertedBy,
                        InsertedAt = p.InsertedAt
                    }).FirstOrDefault();

            return petProfile;
        }

        public PetProfileModel UpdatePetProfile(UpdatePetProfileModel model, Guid updatedBy)
        {
            var petProfile = PrepareUpdate(model, updatedBy);

            Update(petProfile);

            var result = new PetProfileModel
            {
                PetDocumentId = petProfile.PetDocumentId,
                PetName = petProfile.PetName,
                PetGender = petProfile.PetGender,
                PetAge = petProfile.PetAge,
                PetBreedId = petProfile.PetBreedId,
                PetFurColorId = petProfile.PetFurColorId,
                PetImgUrl = petProfile.PetImgUrl,
                PetStatus = petProfile.PetStatus,
                PetProfileDescription = petProfile.PetProfileDescription,
                CenterId = petProfile.CenterId,
                InsertedBy = petProfile.InsertedBy,
                InsertedAt = petProfile.InsertedAt
            };

            return result;
        }
        #endregion

        #region GET BY ID
        public PetProfileModel GetPetProfileById(Guid id)
        {
            var result = Get()
               .Where(p => p.PetDocumentId.Equals(id))
               .Include(p => p.PetBreed)
               .Include(p => p.PetFurColor)
               .Select(p => new PetProfileModel
               {
                   PetDocumentId = p.PetDocumentId,
                   CenterId = p.CenterId,
                   PetProfileDescription = p.PetProfileDescription,
                   PetAge = p.PetAge,
                   PetBreedId = p.PetBreedId,
                   PetBreedName = p.PetBreed.PetBreedName,
                   PetFurColorId = p.PetFurColorId,
                   PetFurColorName = p.PetFurColor.PetFurColorName,
                   PetGender = p.PetGender,
                   PetName = p.PetName,
                   PetStatus = p.PetStatus,
                   PetImgUrl = p.PetImgUrl
               }).FirstOrDefault();
            return result;
        }
        #endregion
    }

}
