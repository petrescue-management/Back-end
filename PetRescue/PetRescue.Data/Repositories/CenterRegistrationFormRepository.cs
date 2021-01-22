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
        SearchReturnModel SearchCenterRegistrationForm(SearchModel moadel);

        CenterRegistrationForm GetCenterRegistrationFormById(Guid id);

        void UpdateCenterRegistrationForm(UpdateCenterRegistrationFormModel model);

        string CreateCenterRegistrationForm(CreateCenterRegistrationFormModel model);

        CenterRegistrationForm UpdateCenterRegistrationStatus(CenterRegistrationForm entity, int status);
        
    }
    public partial class CenterRegistrationFormRepository : BaseRepository<CenterRegistrationForm, string>, ICenterRegistrationFormRepository
    {
        public CenterRegistrationFormRepository(DbContext context) : base(context)
        {
        }

        public SearchReturnModel SearchCenterRegistrationForm(SearchModel model)
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

        public CenterRegistrationForm UpdateCenterRegistrationStatus(CenterRegistrationForm entity, int status)
        {
            entity.CenterRegistrationStatus = status;
            return Update(entity).Entity;
        }
    }
}
