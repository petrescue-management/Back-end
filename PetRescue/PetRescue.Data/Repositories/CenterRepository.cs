﻿using Microsoft.EntityFrameworkCore;
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
                    CenterImageUrl = c.CenterImgUrl
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
                    InsertedAt = c.InsertedAt,
                    UpdatedBy = updatedBy,
                    UpdatedAt = DateTime.Now,
                    CenterImgUrl = c.CenterImgUrl
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
            var old_center = Get().FirstOrDefault(c => c.CenterId.Equals(model.CenterId));
            old_center.CenterStatus = model.CenterStatus;
            old_center.UpdatedAt = DateTime.UtcNow;
            old_center.UpdatedBy = updateBy;
            if(model.CenterAddress != null)
            {
                old_center.Address = model.CenterAddress;
            }
            if(model.CenterName != null)
            {
                old_center.CenterName = model.CenterName;
            }
            if(model.Lat != 0)
            {
                old_center.Lat = model.Lat;
            }
            if (model.Lng != 0)
            {
                old_center.Lng = model.Lng;
            }
            return old_center;
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
                CenterId = model.CenterId,
                Address = model.Address,
                Phone = model.Phone,
                CenterName = model.CenterName,
                CenterStatus = CenterStatusConst.OPENNING,
                Lat = model.Lat,
                Lng = model.Lng,
                InsertedAt = DateTime.Now,
                UpdatedBy = null,
                UpdatedAt = null,
                CenterImgUrl = model.ImageUrl
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
