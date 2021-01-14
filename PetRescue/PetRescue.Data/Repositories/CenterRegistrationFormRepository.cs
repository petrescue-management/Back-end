using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface ICenterRegistrationFormRepository : IBaseRepository<CenterRegistrationForm, string>
    {
        SearchReturnModel SearchCenterRegistrationForm(SearchViewModel model);

        CenterRegistrationForm GetCenterRegistrationFormById(Guid id);

        void UpdateCenterRegistrationForm(UpdateCenterRegistrationFormModel model);

    }
    public partial class CenterRegistrationFormRepository : BaseRepository<CenterRegistrationForm, string>, ICenterRegistrationFormRepository
    {
        public CenterRegistrationFormRepository(DbContext context) : base(context)
        {
        }

        public SearchReturnModel SearchCenterRegistrationForm(SearchViewModel model)
        {
            var records = Get().AsQueryable().Where(f => f.CenterRegisterStatus == 1);

            List<CenterRegistrationForm> result = records
                .Skip((model.PageIndex - 1) * 10)
                .Take(10)
                .Select(f => new CenterRegistrationForm {
                    FormId = f.FormId,
                    CenterName = f.CenterName,
                    Email = f.Email,
                    Phone = f.Phone,
                    CenterAddress = f.CenterAddress,
                    Description = f.Description,
                    CenterRegisterStatus = f.CenterRegisterStatus,
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
                .Where(f => f.FormId.Equals(id))
                .Select(f => new CenterRegistrationForm
                {
                    FormId = f.FormId,
                    CenterName = f.CenterName,
                    Email = f.Email,
                    Phone = f.Phone,
                    CenterAddress = f.CenterAddress,
                    Description = f.Description,
                    CenterRegisterStatus = f.CenterRegisterStatus,
                    UpdatedBy = f.UpdatedBy,
                    UpdatedAt = f.UpdatedAt
                }).FirstOrDefault();
            return result;
        }

        public void UpdateCenterRegistrationForm(UpdateCenterRegistrationFormModel model)
        {
            Update(new CenterRegistrationForm { 
                FormId = model.FormId,
                CenterName = model.CenterName,
                Email = model.Email,
                Phone = model.Phone,
                CenterAddress = model.CenterAddress,
                Description = model.Description,
                CenterRegisterStatus = model.CenterRegisterStatus,
                UpdatedBy = null,
                UpdatedAt = DateTime.Now
            });

            SaveChanges();
        }
    }
}
