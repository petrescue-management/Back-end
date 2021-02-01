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

        CenterModel DeleteCenter(Guid id);


        CenterModel UpdateCenter(UpdateCenterModel model);

        CenterModel CreateCenter(CreateCenterModel model);
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
                    CenterStatus = c.CenterStatus,
                    Phone = c.Phone,
                    InsertAt = c.InsertAt,
                    UpdateAt = c.UpdateAt
                }).FirstOrDefault();

            return result;
        }
        #endregion

        #region DELETE
        private Center PrepareDelete(Guid id)
        {
            var center = Get()
                .Where(c => c.CenterId.Equals(id))
                .Select(c => new Center
                {
                    CenterId = c.CenterId,
                    CenterName = c.CenterName,
                    Address = c.Address,
                    CenterStatus = CenterStatusConst.CLOSED,
                    Phone = c.Phone,
                    InsertBy = c.InsertBy,
                    InsertAt = c.InsertAt,
                    UpdateBy = c.UpdateBy,
                    UpdateAt = DateTime.Now
                }).FirstOrDefault();

            return center;
        }
        public CenterModel DeleteCenter(Guid id)
        {
            var center = PrepareDelete(id);
            Update(center);


            var result = GetResult(center);
            return result;
        }
        #endregion

        #region UPDATE

        private Center PrepareUpdate(UpdateCenterModel model)
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
                UpdateBy = null,
                UpdateAt = DateTime.Now
            };

            return update_center;
        }

        public CenterModel UpdateCenter(UpdateCenterModel model)
        {

            var center = PrepareUpdate(model);
            Update(center);

            var result = GetResult(center);
            return result;
        }

        #endregion

        #region CREATE
        private Center PrepareCreate(CreateCenterModel model)
        {
            var center = new Center
            {
                CenterId = Guid.NewGuid(),
                Address = model.Address,
                Phone = model.Phone,
                CenterName = model.CenterName,
                CenterStatus = CenterStatusConst.OPENNING,
                InsertAt = DateTime.Now,
                InsertBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                UpdateBy = null,
                UpdateAt = null
            };
            return center;
        }

        public CenterModel CreateCenter(CreateCenterModel model)
        {
            var center = PrepareCreate(model);
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
