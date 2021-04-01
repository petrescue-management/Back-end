using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPickerFormRepository : IBaseRepository<PickerForm,string>
    {
        PickerForm Create(PickerFormCreateModel model, Guid insertBy);
    }
    public partial class PickerFormRepository : BaseRepository<PickerForm, string>, IPickerFormRepository
    {
        public PickerFormRepository(DbContext context) : base(context)
        {
        }

        public PickerForm Create(PickerFormCreateModel model, Guid insertBy)
        {
            var pickerForm = PrepareCreate(model, insertBy);
            return Create(pickerForm).Entity;
        }
        private PickerForm PrepareCreate(PickerFormCreateModel model, Guid insertBy)
        {
            var pickerForm = new PickerForm
            {
                //InserAt = DateTime.UtcNow,
                //InsertBy = insertBy,
                PickerDescription = model.PickerDescription,
                PickerImageUrl = model.PickerImageUrl,
                //RescueReportId = model.RescueReportId
            };
            return pickerForm;
        }
    }
}
