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
        Adoption GetAdoptionById(Guid id);

        Adoption UpdateAdoptionStatus(UpdateStatusModel model);

        Adoption CreateAdoption(AdoptionRegistrationFormModel model);
    }

    public partial class AdoptionRepository : BaseRepository<Adoption, string>, IAdoptionRepository
    {
        public AdoptionRepository(DbContext context) : base(context)
        {
        }

        #region GET_BY_ID
        public Adoption GetAdoptionById(Guid id)
        {
            var result = Get()
               .Where(a => a.AdoptionRegistrationId.Equals(id))
               .Select(a => new Adoption
               {
                   AdoptionRegistrationId = a.AdoptionRegistrationId,             
                   AdoptionStatus = a.AdoptionStatus,
                   AdoptedAt = a.AdoptedAt,
                   ReturnedAt = a.ReturnedAt
               }).FirstOrDefault();

            return result;
        }
        #endregion

        #region UPDATE STATUS
        private Adoption PrepareUpdate(UpdateStatusModel model)
        {
            Adoption adoption;

            if (model.Status == AdoptionStatusConst.ADOPTED)
            {
                adoption = Get()
               .Where(a => a.AdoptionRegistrationId.Equals(model.Id))
               .Select(a => new Adoption
               {
                   AdoptionRegistrationId = a.AdoptionRegistrationId,
                   AdoptionStatus = model.Status,
                   AdoptedAt = DateTime.UtcNow,
                   ReturnedAt = null,
                   AdoptionRegistration = a.AdoptionRegistration,
                   InsertedBy = a.InsertedBy,
                   InsertedAt = a.InsertedAt,
                   UpdatedBy = null,
                   UpdatedAt = DateTime.UtcNow
               }).FirstOrDefault();
            }
            else
            {
                adoption = Get()
               .Where(a => a.AdoptionRegistrationId.Equals(model.Id))
               .Select(a => new Adoption
               {
                   AdoptionRegistrationId = a.AdoptionRegistrationId,
                   AdoptionStatus = model.Status,
                   AdoptedAt = a.AdoptedAt,
                   ReturnedAt = DateTime.UtcNow,
                   AdoptionRegistration = a.AdoptionRegistration,
                   InsertedBy = a.InsertedBy,
                   InsertedAt = a.InsertedAt,
                   UpdatedBy = null,
                   UpdatedAt = DateTime.UtcNow
               }).FirstOrDefault();
            }
            
            return adoption;
        }
        public Adoption UpdateAdoptionStatus(UpdateStatusModel model)
        {
            Adoption adoption = PrepareUpdate(model);         

            Update(adoption);

            return adoption;
        }

        #endregion

        #region CREATE
        private Adoption PrepareCreate(AdoptionRegistrationFormModel model)
        {

            var adoption = new Adoption
            {
                AdoptionRegistrationId = model.AdoptionRegistrationId,
                AdoptionStatus = 1,
                InsertedBy = (Guid)model.UpdatedBy,
                InsertedAt = DateTime.UtcNow,
                UpdatedBy = null,
                UpdatedAt = null,
                AdoptedAt = DateTime.UtcNow
            };
            return adoption;
        }

        public Adoption CreateAdoption(AdoptionRegistrationFormModel model)
        {
            var adoption = PrepareCreate(model);
            Create(adoption);

            return adoption;
        }
        #endregion

    
    }

}
