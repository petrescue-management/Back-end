using Microsoft.EntityFrameworkCore;
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
        SearchReturnModel SearchCenter(SearchViewModel model);

        Center GetCenterById(Guid id);

        void DeleteCenter(Guid id);

        void CreateCenter(CreateCenterModel model);

        string UpdateCenter(UpdateCenterModel model);
    }

    public partial class CenterRepository : BaseRepository<Center, string>, ICenterRepository
    {
        public CenterRepository(DbContext context) : base(context)
        {
        }

        public SearchReturnModel SearchCenter(SearchViewModel model)
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
            //check Duplicate  phone
            var check_dup_phone = Get()
                .Where(c => c.Phone.Equals(model.Phone));

            //check Duplicate address
            var check_dup_address = Get()
               .Where(c => c.Address.Equals(model.Address));

            //dup phone & address
            if (check_dup_phone.Any() && check_dup_address.Any())
                return "This phone and address  is already registered !";

            //dup phone
            if (check_dup_phone.Any())
                return "This phone is already registered !";

            //dup address
            if (check_dup_address.Any())
                return "This address is already registered !";


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
    }
}
