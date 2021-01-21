using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface ICenterRegistrationFormRepository : IBaseRepository<CenterRegistrationForm, string>
    {
        SearchReturnModel SearchCenterRegistrationForm(SearchViewModel model);

        CenterRegistrationForm GetCenterRegistrationFormById(Guid id);

        void UpdateCenterRegistrationForm(UpdateCenterRegistrationFormModel model);

        string CreateCenterRegistrationForm(CreateCenterRegistrationFormModel model);

    }
    public partial class CenterRegistrationFormRepository : BaseRepository<CenterRegistrationForm, string>, ICenterRegistrationFormRepository
    {
        public CenterRegistrationFormRepository(DbContext context) : base(context)
        {
        }

        public SearchReturnModel SearchCenterRegistrationForm(SearchViewModel model)
        {
            var records = Get().AsQueryable().Where(f => f.CenterRegistrationStatus == 1);

            List<CenterRegistrationForm> result = records
                .Skip((model.PageIndex - 1) * 10)
                .Take(10)
                .Select(f => new CenterRegistrationForm {
                    CenterRegistrationId = f.CenterRegistrationId,
                    CenterName = f.CenterName,
                    Email = f.Email,
                    Phone = f.Phone,
                    CenterAddress = f.CenterAddress,
                    Description = f.Description,
                    CenterRegistrationStatus = f.CenterRegistrationStatus,
                    UpdatedBy = f.UpdatedBy,
                    UpdatedAt = f.UpdatedAt
                }).ToList();
            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }

        public CenterRegistrationForm GetCenterRegistrationFormById(Guid id)
        {
            var result = Get()
                .Where(f => f.CenterRegistrationId.Equals(id))
                .Select(f => new CenterRegistrationForm
                {
                    CenterRegistrationId = f.CenterRegistrationId,
                    CenterName = f.CenterName,
                    Email = f.Email,
                    Phone = f.Phone,
                    CenterAddress = f.CenterAddress,
                    Description = f.Description,
                    CenterRegistrationStatus = f.CenterRegistrationStatus,
                    UpdatedBy = f.UpdatedBy,
                    UpdatedAt = f.UpdatedAt
                }).FirstOrDefault();
            return result;
        }

        public void UpdateCenterRegistrationForm(UpdateCenterRegistrationFormModel model)
        {
            var form = Get()
                .Where(f => f.CenterRegistrationId.Equals(model.FormId))
                .Select(f => new CenterRegistrationForm
                {
                    CenterName = f.CenterName,
                    Phone = f.Phone,
                    Email = f.Email,
                    CenterAddress = f.CenterAddress,
                    Description = f.Description
                }).FirstOrDefault();

            Update(new CenterRegistrationForm { 
                CenterRegistrationId = model.FormId,
                CenterName = form.CenterName,
                Email = form.Email,
                Phone = form.Phone,
                CenterAddress = form.CenterAddress,
                Description = form.Description,
                CenterRegistrationStatus = model.CenterRegisterStatus,
                UpdatedBy = null,
                UpdatedAt = DateTime.Now
            });

            SaveChanges();
        }

        public string CreateCenterRegistrationForm(CreateCenterRegistrationFormModel model)
        {
            //call dbset_User
            var user_dbset = context.Set<User>();

            //call dbset_Center
            var center_dbset = context.Set<Center>();

            //check Duplicate  phone
            var check_dup_phone = center_dbset.AsQueryable()
                .Where(c => c.Phone.Equals(model.Phone));

            //check Duplicate email
            var check_dup_email = user_dbset.AsQueryable()
               .Where(u => u.UserEmail.Equals(model.Email));

            //check Duplicate address
            var check_dup_address = center_dbset.AsQueryable()
               .Where(c => c.Address.Equals(model.CenterAddress));

            //dup phone & email
            if (check_dup_phone.Any() && check_dup_email.Any())
                return "This phone and email  is already registered !";

            //dup phone & address
            if (check_dup_phone.Any() && check_dup_address.Any())
                return "This phone and address  is already registered !";

            //dup email & address
            if (check_dup_email.Any() && check_dup_address.Any())
                return "This email and address  is already registered !";

            //dup phone
            if (check_dup_phone.Any())
                return "This phone is already registered !";

            //dup email
            if (check_dup_email.Any())
                return "This email is already registered !";

            //dup address
            if (check_dup_address.Any())
                return "This address is already registered !";

            Create(new CenterRegistrationForm
            {
                CenterRegistrationId = Guid.NewGuid(),
                CenterName = model.CenterName,
                Email = model.Email,
                Phone = model.Phone,
                CenterAddress = model.CenterAddress,
                Description = model.Description,
                CenterRegistrationStatus = 1,
                UpdatedBy = null,
                UpdatedAt = null
            });

            SaveChanges();
            return "This center registration form is processing !";
        }
    }
}
