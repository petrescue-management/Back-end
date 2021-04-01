using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class PickerFormDomain : BaseDomain
    {
        public PickerFormDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public PickerFormViewModel Create(PickerFormCreateModel model, Guid insertBy)
        {
            var pickerRepo = uow.GetService<IPickerFormRepository>();
            var result = pickerRepo.Create(model, insertBy);
            uow.saveChanges();
            if(result != null)
            {
                return new PickerFormViewModel
                {
                    PickerDescription = result.PickerDescription,
                    PickerImageUrl = result.PickerImageUrl
                };
            }
            return null;
            
        }
    }
}
