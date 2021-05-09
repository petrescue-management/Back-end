
﻿using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Extensions;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PetRescue.Data.Domains
{
    public class PickerFormDomain : BaseDomain
    {
        private readonly IPickerFormRepository _pickerFormRepo;
        public PickerFormDomain(IUnitOfWork uow, 
            IPickerFormRepository pickerFormRepo) : base(uow)
        {
            this._pickerFormRepo = pickerFormRepo;
        }
        #region SEARCH
        public SearchReturnModel SearchPickerForm(SearchModel model)
        {
            var records = _pickerFormRepo.Get().AsQueryable();
            List<PickerFormModel> result = records
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(p => new PickerFormModel
                {
                    PickerFormId = p.PickerFormId,
                    Description = p.Description,
                    PickerImageUrl = p.PickerFormImgUrl,
                    InsertedBy = p.InsertedBy,
                    InsertedAt = p.InsertedAt
                }).ToList();
            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }
        #endregion

        #region GET BY ID
        public PickerFormModel GetPickerFormById(Guid id)
        {
            return _pickerFormRepo.GetPickerFormById(id);
        }
        #endregion
        #region UPDATE STATUS
        public PickerFormModel UpdatePickerFormStatus(UpdateStatusModel model, Guid updatedBy)
        {
            var pickerForm = _pickerFormRepo.UpdatePickerFormStatus(model, updatedBy);
            if(pickerForm != null)
            {
                _uow.SaveChanges();
                return pickerForm;
            }
            return null;
        }
        #endregion

        #region CREATE
        public PickerFormModel CreatePickerForm(CreatePickerFormModel model, Guid insertedBy)
        {
            var pickerForm = _pickerFormRepo.CreatePickerForm(model, insertedBy);
            if(pickerForm != null)
            {
                _uow.SaveChanges();
                return pickerForm;
            }
            return null;
        }

        #endregion
    }
}
