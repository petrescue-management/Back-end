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

        CenterModel DeleteCenter(Guid id, Guid updateBy);


        CenterModel UpdateCenter(UpdateCenterModel model, Guid updateBy);

        CenterModel CreateCenter(CreateCenterModel model, Guid insertBy);
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
                    Long = c.Long,
                    CenterStatus = c.CenterStatus,
                    Phone = c.Phone,
                    InsertAt = c.InsertAt,
                    UpdateAt = c.UpdateAt
                }).FirstOrDefault();

            return result;
        }
        #endregion

        #region DELETE
        private Center PrepareDelete(Guid id, Guid updateBy)
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
                    Long = c.Long,
                    Phone = c.Phone,
                    InsertBy = c.InsertBy,
                    InsertAt = c.InsertAt,
                    UpdateBy = updateBy,
                    UpdateAt = DateTime.Now
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
                   InsertBy = c.InsertBy,
                   InsertAt = c.InsertAt
               }).FirstOrDefault();

            var update_center = new Center
            {
                CenterId = model.CenterId,
                CenterName = model.CenterName,
                Address = model.Address,
                CenterStatus = model.CenterStatus,
                Phone = model.Phone,
                InsertBy = old_center.InsertBy,
                InsertAt = old_center.InsertAt,
                UpdateBy = updateBy,
                UpdateAt = DateTime.Now,
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
        private Center PrepareCreate(CreateCenterModel model, Guid insertBy)
        {
            var center = new Center
            {
                CenterId = Guid.NewGuid(),
                Address = model.Address,
                Phone = model.Phone,
                CenterName = model.CenterName,
                CenterStatus = CenterStatusConst.OPENNING,
                Lat = model.Lat,
                Long = model.Long,
                InsertAt = DateTime.Now,
                InsertBy = insertBy,
                UpdateBy = null,
                UpdateAt = null
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
                Long = center.Long,
                CenterStatus = center.CenterStatus,
                Phone = center.Phone,
                InsertAt = center.InsertAt,
                UpdateAt = center.UpdateAt
            };
            return result;
        }
        #endregion
    }
}
