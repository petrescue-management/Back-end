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
        SearchReturnModel SearchCenter(SearchModel model);

        Center GetCenterById(Guid id);

        void DeleteCenter(Guid id);

        void CreateCenter(CreateCenterModel model);

        string UpdateCenter(UpdateCenterModel model);

        Center CreateCenterByForm(CreateCenterModel model);
    }

    public partial class CenterRepository : BaseRepository<Center, string>, ICenterRepository
    {
        public CenterRepository(DbContext context) : base(context)
        {
        }

        public SearchReturnModel SearchCenter(SearchModel model)
        {
            var records = Get().AsQueryable();

            if (!string.IsNullOrEmpty(model.Keyword) && !string.IsNullOrWhiteSpace(model.Keyword))
                records = records.Where(c => c.CenterName.Contains(model.Keyword));

            List<Center> result = records
                .Skip((model.PageIndex - 1) * 10)
                .Take(10)
                .Select(c => new Center
                {
                    CenterId = c.CenterId,
                    CenterName = c.CenterName,
                    Address = c.Address,
                    CenterStatus = c.CenterStatus,
                    Phone = c.Phone,
                    InsertBy = c.InsertBy,
                    InsertAt = c.InsertAt,
                    UpdateBy = c.UpdateBy,
                    UpdateAt = c.UpdateAt
                }).ToList();
            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }

        public Center GetCenterById(Guid id)
        {
            var result = Get()
                .Where(c => c.CenterId.Equals(id))
                .Select(c => new Center
                {
                    CenterId = c.CenterId,
                    CenterName = c.CenterName,
                    Address = c.Address,
                    CenterStatus = c.CenterStatus,
                    Phone = c.Phone,
                    InsertBy = c.InsertBy,
                    InsertAt = c.InsertAt,
                    UpdateBy = c.UpdateBy,
                    UpdateAt = c.UpdateAt
                }).FirstOrDefault();

            return result;
        }

        public void DeleteCenter(Guid id)
        {
            var center = Get()
                .Where(c => c.CenterId.Equals(id))
                .Select(c => new Center
                {
                    CenterId = c.CenterId,
                    CenterName = c.CenterName,
                    Address = c.Address,
                    CenterStatus = 3,
                    Phone = c.Phone,
                    InsertBy = c.InsertBy,
                    InsertAt = c.InsertAt,
                    UpdateBy = c.UpdateBy,
                    UpdateAt = DateTime.Now
                }).FirstOrDefault();
            Update(center);
            SaveChanges();
        }

        public void CreateCenter(CreateCenterModel model)
        {
            Create(new Center
            {
                CenterId = Guid.NewGuid(),
                CenterName = model.CenterName,
                Address = model.Address,
                CenterStatus = 1,
                Phone = model.Phone,
                InsertBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                InsertAt = DateTime.Now,
                UpdateBy = null,
                UpdateAt = null
            });
            SaveChanges();
        }

        public string UpdateCenter(UpdateCenterModel model)
        {
            var center = Get()
              .Where(c => c.CenterId.Equals(model.CenterId))
               .Select(c => new Center
               {
                   InsertBy = c.InsertBy,
                   InsertAt = c.InsertAt
               }).FirstOrDefault();

            Update(new Center
            {
                CenterId = model.CenterId,
                CenterName = model.CenterName,
                Address = model.Address,
                CenterStatus = model.CenterStatus,
                Phone = model.Phone,
                InsertBy = center.InsertBy,
                InsertAt = center.InsertAt,
                UpdateBy = null,
                UpdateAt = DateTime.Now
            });
            SaveChanges();

            return "Updated your information successfully !";
        }

        public Center CreateCenterByForm(CreateCenterModel model)
        {
            var newCenter = new Center
            {
                CenterId = Guid.NewGuid(),
                Address = model.Address,
                Phone = model.Phone,
                CenterName = model.CenterName,
                CenterStatus = CenterStatus.OPENNING,
                InsertAt = DateTime.Now,
                InsertBy = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                UpdateBy = null,
                UpdateAt = null
            };
            Create(newCenter);
            return newCenter;
        }
    }
}
