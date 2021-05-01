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

        CenterRegistrationForm UpdateCenterRegistrationStatus(CenterRegistrationForm form,UpdateRegistrationCenter model, Guid insertBy);
        
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
                    Lat = f.Lat,
                    Lng = f.Lng,
                    CenterAddress = f.CenterAddress,
                    Description = f.Description,
                    CenterRegistrationStatus = f.CenterRegistrationStatus,
                    UpdatedAt = f.UpdatedAt,
                    ImageUrl = f.ImageUrl,
                    InsertedAt = f.InsertedAt
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
                Lat = model.Lat,
                Lng = model.Lng,
                CenterAddress = model.CenterAddress,
                Description = model.Description,
                CenterRegistrationStatus = 1,
                UpdatedAt = null,
                ImageUrl = model.ImageUrl
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
        private CenterRegistrationForm PrepareUpdate(CenterRegistrationForm form,UpdateRegistrationCenter model, Guid updateBy)
        {
            form.CenterRegistrationStatus = model.Status;
            form.UpdatedAt = DateTime.UtcNow;
            return form;
        }

        public CenterRegistrationForm UpdateCenterRegistrationStatus(CenterRegistrationForm form, UpdateRegistrationCenter model, Guid updateBy)
        {
            form = PrepareUpdate(form,model, updateBy);
            return Update(form).Entity;
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
                Lat = form.Lat,
                Lng = form.Lng,
                CenterAddress = form.CenterAddress,
                Description = form.Description,
                CenterRegistrationStatus = form.CenterRegistrationStatus,
                UpdatedAt = form.UpdatedAt,
                ImageUrl = form.ImageUrl   
            };
            return result;
        }
        #endregion
    }
}
