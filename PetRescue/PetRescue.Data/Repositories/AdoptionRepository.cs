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
        AdoptionModel GetAdoptionById(Guid id);

        AdoptionModel UpdateAdoptionStatus(UpdateStatusModel model);

        AdoptionModel CreateAdoption(AdoptionRegisterFormModel model);
    }

    public partial class AdoptionRepository : BaseRepository<Adoption, string>, IAdoptionRepository
    {
        public AdoptionRepository(DbContext context) : base(context)
        {
        }

        #region GET_BY_ID
        public AdoptionModel GetAdoptionById(Guid id)
        {
            var result = Get()
               .Where(a => a.AdoptionRegisterId.Equals(id))
               .Select(a => new AdoptionModel
               {
                   AdoptionRegisterId = a.AdoptionRegisterId,
                   OwnerId = a.AdoptionRegister.InsertedBy,
                   OwnerName = a.AdoptionRegister.UserName,
                   PetId = a.AdoptionRegister.PetId,
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
               .Where(a => a.AdoptionRegisterId.Equals(model.Id))
               .Select(a => new Adoption
               {
                   AdoptionRegisterId = a.AdoptionRegisterId,
                   AdoptionStatus = model.Status,
                   AdoptedAt = DateTime.UtcNow,
                   ReturnedAt = null,
                   AdoptionRegister = a.AdoptionRegister,
                   InsertedBy = a.InsertedBy,
                   InsertedAt = a.InsertedAt,
                   UpdatedBy = null,
                   UpdatedAt = DateTime.UtcNow
               }).FirstOrDefault();
            }
            else
            {
                adoption = Get()
               .Where(a => a.AdoptionRegisterId.Equals(model.Id))
               .Select(a => new Adoption
               {
                   AdoptionRegisterId = a.AdoptionRegisterId,
                   AdoptionStatus = model.Status,
                   AdoptedAt = a.AdoptedAt,
                   ReturnedAt = DateTime.UtcNow,
                   AdoptionRegister = a.AdoptionRegister,
                   InsertedBy = a.InsertedBy,
                   InsertedAt = a.InsertedAt,
                   UpdatedBy = null,
                   UpdatedAt = DateTime.UtcNow
               }).FirstOrDefault();
            }
            
            return adoption;
        }
        public AdoptionModel UpdateAdoptionStatus(UpdateStatusModel model)
        {
            Adoption adoption = PrepareUpdate(model);         

            Update(adoption);

            var result = GetResult(adoption);

            return result;
        }

        #endregion

        #region CREATE
        private Adoption PrepareCreate(AdoptionRegisterFormModel model)
        {

            var adoption = new Adoption
            {
                AdoptionRegisterId = model.AdoptionRegisterId,
                AdoptionStatus = 1,
                InsertedBy = Guid.Parse(model.UpdatedBy.ToString()),
                InsertedAt = DateTime.UtcNow,
                UpdatedBy = null,
                UpdatedAt = null
            };
            return adoption;
        }

        public AdoptionModel CreateAdoption(AdoptionRegisterFormModel model)
        {
            var adoption = PrepareCreate(model);
            Create(adoption);

            var result = GetResult(adoption);
            return result;
        }
        #endregion

        #region GET RESULT
        private AdoptionModel GetResult(Adoption adoption)
        {
            var result = new AdoptionModel
            {
                AdoptionRegisterId = adoption.AdoptionRegisterId,
                OwnerId = adoption.AdoptionRegister.InsertedBy,
                OwnerName = adoption.AdoptionRegister.UserName,
                PetId = adoption.AdoptionRegister.PetId,
                AdoptionStatus = adoption.AdoptionStatus,
                AdoptedAt = adoption.AdoptedAt,
                ReturnedAt = adoption.ReturnedAt
            };
            return result;
        }
        #endregion
    }

}
