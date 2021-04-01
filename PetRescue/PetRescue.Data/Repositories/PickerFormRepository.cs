using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{

    public partial interface IPickerFormRepository : IBaseRepository<PickerForm, string>
    {

        PickerFormModel GetPickerFormById(Guid id);

        PickerFormModel UpdatePickerFormStatus(UpdateStatusModel model, Guid updateBy);

        PickerFormModel CreatePickerForm(CreatePickerFormModel model, Guid insertedBy);
    }
    public partial class PickerFormRepository : BaseRepository<PickerForm, string>, IPickerFormRepository
    {
        public PickerFormRepository(DbContext context) : base(context)
        {
        }
        #region CREATE
        private PickerForm PrepareCreate(CreatePickerFormModel model, Guid insertedBy)
        {

            var pickerForm = new PickerForm
            {
                PickerFormId = Guid.NewGuid(),
                PickerDescription = model.PickerDescription,
                PickerImageUrl = model.PickerImageUrl,
                InsertedBy = insertedBy,
                InsertedAt = DateTime.UtcNow
            };
            return pickerForm;
        }


        public PickerFormModel CreatePickerForm(CreatePickerFormModel model, Guid insertedBy)
        {
            var pickerForm = PrepareCreate(model, insertedBy);

            Create(pickerForm);

            var result = new PickerFormModel
            {
                PickerFormId = pickerForm.PickerFormId,
                PickerDescription = pickerForm.PickerDescription,
                PickerImageUrl = pickerForm.PickerImageUrl,
                InsertedBy = pickerForm.InsertedBy,
                InsertedAt = pickerForm.InsertedAt
            };
            return result;

        }
        #endregion

        #region GET BY ID
        public PickerFormModel GetPickerFormById(Guid id)
        {
            var result = Get()
                .Where(p => p.PickerFormId.Equals(id))
                .Select(p => new PickerFormModel
                {
                    PickerFormId = p.PickerFormId,
                    PickerDescription = p.PickerDescription,
                    PickerImageUrl = p.PickerImageUrl,
                    InsertedBy = p.InsertedBy,
                    InsertedAt = p.InsertedAt
                }).FirstOrDefault();
            return result;
        }
        #endregion

        #region UPDATE STATUS
        private PickerForm PrepareUpdate(UpdateStatusModel model, Guid updatedBy)
        {
            var pickerForm = Get()
                    .Where(p => p.PickerFormId.Equals(model.Id))
                    .Select(p => new PickerForm
                    {
                        PickerFormId = model.Id,
                        PickerDescription = p.PickerDescription,
                        PickerImageUrl = p.PickerImageUrl,
                        InsertedBy = p.InsertedBy,
                        InsertedAt = p.InsertedAt
                    }).FirstOrDefault();

            return pickerForm;
        }
       public PickerFormModel UpdatePickerFormStatus(UpdateStatusModel model, Guid updatedBy)
       {
            var pickerForm = PrepareUpdate(model, updatedBy);

            Update(pickerForm);

            var result = Get()
                    .Where(p => p.PickerFormId.Equals(model.Id))
                    .Select(p => new PickerFormModel
                    {
                        PickerFormId = p.PickerFormId,
                        PickerDescription = p.PickerDescription,
                        PickerImageUrl = p.PickerImageUrl,
                        InsertedBy = p.InsertedBy,
                        InsertedAt = p.InsertedAt
                    }).FirstOrDefault();
            
            return result;
       }
       #endregion
    }
}
