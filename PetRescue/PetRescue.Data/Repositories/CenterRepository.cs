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
    public partial interface ICenterRepository : IBaseRepository<Center, string>
    {

        CenterModel GetCenterById(Guid id);

        CenterModel DeleteCenter(Guid id, Guid updatedBy);


        CenterModel UpdateCenter(UpdateCenterModel model, Guid updatedBy);

        CenterModel CreateCenter(CreateCenterModel model, Guid insertedBy);
    }

    public partial class CenterRepository : BaseRepository<Center, string>, ICenterRepository
    {
        public CenterRepository(DbContext context) : base(context)
        {
        }

        #region GET BY ID
        public CenterModel GetCenterById(Guid id)
        {
            var result = Get()
                .Where(c => c.CenterId.Equals(id))
                .Select(c => new CenterModel
                {
                    CenterId = c.CenterId,
                    CenterName = c.CenterName,
                    Address = c.Address,
                    Lat = c.Lat,
                    Long = c.Lng,
                    CenterStatus = c.CenterStatus,
                    Phone = c.Phone,
                    InsertedAt = c.InsertedAt,
                    UpdatedAt = c.UpdatedAt,
                    ImageUrl = c.ImageUrl
                }).FirstOrDefault();

            return result;
        }
        #endregion

        #region DELETE
        private Center PrepareDelete(Guid id, Guid updatedBy)
        {
            var center = Get()
                .Where(c => c.CenterId.Equals(id))
                .Select(c => new Center
                {
                    CenterId = c.CenterId,
                    CenterName = c.CenterName,
                    Address = c.Address,
                    CenterStatus = CenterStatusConst.CLOSED,
                    Lat = c.Lat,
                    Lng = c.Lng,
                    Phone = c.Phone,
                    InsertedBy = c.InsertedBy,
                    InsertedAt = c.InsertedAt,
                    UpdatedBy = updatedBy,
                    UpdatedAt = DateTime.Now,
                    ImageUrl = c.ImageUrl
                }).FirstOrDefault();

            return center;
        }
        public CenterModel DeleteCenter(Guid id, Guid updateBy)
        {
            var center = PrepareDelete(id, updateBy);
            Update(center);
            var result = GetResult(center);
            return result;
        }
        #endregion

        #region UPDATE

        private Center PrepareUpdate(UpdateCenterModel model, Guid updateBy)
        {
            var old_center = Get()
              .Where(c => c.CenterId.Equals(model.CenterId))
               .Select(c => new Center
               {
                   InsertedBy = c.InsertedBy,
                   InsertedAt = c.InsertedAt
               }).FirstOrDefault();

            var update_center = new Center
            {
                CenterId = model.CenterId,
                CenterName = model.CenterName,
                Address = model.Address,
                CenterStatus = model.CenterStatus,
                Phone = model.Phone,
                InsertedBy = old_center.InsertedBy,
                InsertedAt = old_center.InsertedAt,
                UpdatedBy = updateBy,
                UpdatedAt = DateTime.Now,
                ImageUrl = old_center.ImageUrl
            };

            return update_center;
        }

        public CenterModel UpdateCenter(UpdateCenterModel model, Guid updateBy)
        {

            var center = PrepareUpdate(model, updateBy);
            Update(center);
            var result = GetResult(center);
            return result;
        }

        #endregion

        #region CREATE
        private Center PrepareCreate(CreateCenterModel model, Guid insertedBy)
        {
            var center = new Center
            {
                CenterId = Guid.NewGuid(),
                Address = model.Address,
                Phone = model.Phone,
                CenterName = model.CenterName,
                CenterStatus = CenterStatusConst.OPENNING,
                Lat = model.Lat,
                Lng = model.Lng,
                InsertedAt = DateTime.Now,
                InsertedBy = insertedBy,
                UpdatedBy = null,
                UpdatedAt = null,
                ImageUrl = model.ImageUrl
            };
            return center;
        }

        public CenterModel CreateCenter(CreateCenterModel model, Guid insertBy)
        {
            var center = PrepareCreate(model, insertBy);
            Create(center);

            var result = GetResult(center);

            return result;
        }
        #endregion

        #region GET RESULT
        private CenterModel GetResult(Center center)
        {
            var result = new CenterModel
            {
                CenterId = center.CenterId,
                CenterName = center.CenterName,
                Address = center.Address,
                Lat = center.Lat,
                Long = center.Lng,
                CenterStatus = center.CenterStatus,
                Phone = center.Phone,
                InsertedAt = center.InsertedAt,
                UpdatedAt = center.UpdatedAt
            };
            return result;
        }
        #endregion
    }
}
