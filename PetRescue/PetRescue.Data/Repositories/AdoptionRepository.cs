using Microsoft.EntityFrameworkCore;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IAdoptionRepository : IBaseRepository<Adoption, string>
    {
        Adoption GetAdoptionById(Guid adoptionRegistrationFormId);

        Adoption UpdateAdoptionStatus(CancelModel model, Guid updatedBy);

        Adoption CreateAdoption(Guid adoptionRegistrationFormId, Guid insertedBy);
    }

    public partial class AdoptionRepository : BaseRepository<Adoption, string>, IAdoptionRepository
    {
        public AdoptionRepository(DbContext context) : base(context)
        {
        }

        #region GET_BY_ID
        public Adoption GetAdoptionById(Guid adoptionRegistrationFormId)
        {
            var result = Get()
               .Where(a => a.AdoptionRegistrationId.Equals(adoptionRegistrationFormId))
               .Select(a => new Adoption
               {
                   AdoptionRegistrationId = a.AdoptionRegistrationId,             
                   AdoptionStatus = a.AdoptionStatus,
                   InsertedAt = a.InsertedAt,
                   UpdatedAt = a.UpdatedAt
               }).FirstOrDefault();

            return result;
        }
        #endregion

        #region UPDATE STATUS
        private Adoption PrepareUpdate(CancelModel model, Guid updatedBy)
        {
            Adoption adoption = Get()
               .Where(a => a.AdoptionRegistrationId.Equals(model.Id))
               .Select(a => new Adoption
               {
                   AdoptionRegistrationId = a.AdoptionRegistrationId,
                   AdoptionStatus = model.Status,
                   AdoptionRegistration = a.AdoptionRegistration,
                   ReturnedReason = model.Reason,
                   InsertedBy = a.InsertedBy,
                   InsertedAt = a.InsertedAt,
                   UpdatedBy = updatedBy,
                   UpdatedAt = DateTime.UtcNow
               }).FirstOrDefault();
            
            return adoption;
        }
        public Adoption UpdateAdoptionStatus(CancelModel model, Guid updatedBy)
        {
            Adoption adoption = PrepareUpdate(model, updatedBy);         

            Update(adoption);

            return adoption;
        }

        #endregion

        #region CREATE
        private Adoption PrepareCreate(Guid adoptionRegistrationFormId, Guid insertedBy)
        {

            var adoption = new Adoption
            {
                AdoptionRegistrationId = adoptionRegistrationFormId,
                AdoptionStatus = AdoptionStatusConst.ADOPTED,
                ReturnedReason = null,
                InsertedBy = insertedBy,
                InsertedAt = DateTime.UtcNow,
                UpdatedBy = null,
                UpdatedAt = null
            };
            return adoption;
        }

        public Adoption CreateAdoption(Guid adoptionRegistrationFormId, Guid insertedBy)
        {
            var adoption = PrepareCreate(adoptionRegistrationFormId, insertedBy);
            Create(adoption);

            return adoption;
        }
        #endregion

    
    }

}
