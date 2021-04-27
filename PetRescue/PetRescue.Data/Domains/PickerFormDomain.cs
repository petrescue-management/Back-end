
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
        public PickerFormDomain(IUnitOfWork uow) : base(uow)
        {
        }
        #region SEARCH
        public SearchReturnModel SearchPickerForm(SearchModel model)
        {
            var records = uow.GetService<IPickerFormRepository>().Get().AsQueryable();


            List<PickerFormModel> result = records
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(p => new PickerFormModel
                {
                    PickerFormId = p.PickerFormId,
                    PickerDescription = p.PickerDescription,
                    PickerImageUrl = p.PickerImageUrl,
                    InsertedBy = p.InsertedBy,
                    InsertedAt = p.InsertedAt.AddHours(ConstHelper.UTC_VIETNAM)
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
            var pickerForm = uow.GetService<IPickerFormRepository>().GetPickerFormById(id);
            return pickerForm;
        }
        #endregion

        #region UPDATE STATUS
        public PickerFormModel UpdatePickerFormStatus(UpdateStatusModel model, Guid updatedBy)
        {
            var pickerForm = uow.GetService<IPickerFormRepository>().UpdatePickerFormStatus(model, updatedBy);
            uow.saveChanges();
            return pickerForm;
        }
        #endregion

        #region CREATE
        public PickerFormModel CreatePickerForm(CreatePickerFormModel model, Guid insertedBy)
        {
            var pickerForm = uow.GetService<IPickerFormRepository>().CreatePickerForm(model, insertedBy);
            uow.saveChanges();
            return pickerForm;
        }

        #endregion
    }
}
