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

        CenterRegistrationFormModel GetCenterRegistrationFormById(Guid id);

        CenterRegistrationFormModel CreateCenterRegistrationForm(CreateCenterRegistrationFormModel model);

        CenterRegistrationFormModel UpdateCenterRegistrationStatus(UpdateStatusModel model);
        
    }
    public partial class CenterRegistrationFormRepository : BaseRepository<CenterRegistrationForm, string>, ICenterRegistrationFormRepository
    {
        public CenterRegistrationFormRepository(DbContext context) : base(context)
        {
        }

        #region GET BY ID
        public CenterRegistrationFormModel GetCenterRegistrationFormById(Guid id)
        {
            var result = Get()
                .Where(f => f.CenterRegistrationId.Equals(id))
                .Select(f => new CenterRegistrationFormModel
                {
                    CenterRegistrationId = f.CenterRegistrationId,
                    CenterName = f.CenterName,
                    Email = f.Email,
                    Phone = f.Phone,
                    CenterAddress = f.CenterAddress,
                    Description = f.Description,
                    CenterRegistrationStatus = f.CenterRegistrationStatus,
                    UpdatedAt = f.UpdatedAt
                }).FirstOrDefault();
            return result;
        }
        #endregion

        #region CREATE
        private CenterRegistrationForm PrepareCreate(CreateCenterRegistrationFormModel model)
        {
            var form = new CenterRegistrationForm
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
            };

            return form;
        }

        public CenterRegistrationFormModel CreateCenterRegistrationForm(CreateCenterRegistrationFormModel model)
        {
            var form = PrepareCreate(model);
            Create(form);

            var result = GetResult(form);

            return result;
        }
        #endregion

        #region UPDATE STATUS
        private CenterRegistrationForm PrepareUpdate(UpdateStatusModel model)
        {
            var form = Get()
                  .Where(r => r.CenterRegistrationId.Equals(model.Id))
                  .Select(r => new CenterRegistrationForm
                  {
                      CenterRegistrationId = model.Id,
                      CenterName = r.CenterName,
                      Email = r.Email,
                      Phone = r.Phone,
                      CenterAddress = r.CenterAddress,
                      Description = r.Description,
                      CenterRegistrationStatus = model.Status,
                      UpdatedBy = null,
                      UpdatedAt = null
                  }).FirstOrDefault();

            return form;
        }

        public CenterRegistrationFormModel UpdateCenterRegistrationStatus(UpdateStatusModel model)
        {

            var form = PrepareUpdate(model);

            Update(form);

            var result = GetResult(form);

            return result;
        }
        #endregion

        #region GET RESULT
        private CenterRegistrationFormModel GetResult(CenterRegistrationForm form)
        {
            var result = new CenterRegistrationFormModel
            {
                CenterRegistrationId = form.CenterRegistrationId,
                CenterName = form.CenterName,
                Email = form.Email,
                Phone = form.Phone,
                CenterAddress = form.CenterAddress,
                Description = form.Description,
                CenterRegistrationStatus = form.CenterRegistrationStatus,
                UpdatedAt = form.UpdatedAt
            };
            return result;
        }
        #endregion
    }
}
