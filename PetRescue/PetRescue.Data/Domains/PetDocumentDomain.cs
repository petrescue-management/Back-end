using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class PetDocumentDomain : BaseDomain
    {
        public PetDocumentDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public List<PetDocumentModel> GetListPetDocumentByCenterId(Guid centerId)
        {
            var petDocumentRepo = uow.GetService<IPetDocumentRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var petDocuments = petDocumentRepo.Get().Where(s => s.CenterId.Equals(centerId));
            var result = new List<PetDocumentModel>();
            foreach(var petDocument in petDocuments)
            {
                var currentUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(petDocument.FinderForm.InsertedBy));
                var finderForm = new FinderFormViewModel
                {
                    FinderDate = petDocument.FinderForm.InsertedAt,
                    FinderDescription = petDocument.FinderForm.FinderDescription,
                    FinderImageUrl = petDocument.FinderForm.FinderFormImgUrl,
                    FinderName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName,
                    Lat = petDocument.FinderForm.Lat,
                    Lng = petDocument.FinderForm.Lng,
                };
                currentUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(petDocument.PickerForm.InsertedBy));
                var pickerForm = new PickerFormViewModel
                {
                    PickerDate = petDocument.PickerForm.InsertedAt,
                    PickerDescription = petDocument.PickerForm.PickerDescription,
                    PickerImageUrl = petDocument.PickerForm.PickerImageUrl,
                    PickerName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName,
                };
                result.Add(new PetDocumentModel 
                {
                    FinderForm = finderForm,
                    PetDocumentId = petDocument.PetDocumentId,
                    PickerForm = pickerForm,
                    PetDocumentStatus = petDocument.PetDocumentStatus
                });
            }
            return result;
        }
        public bool Edit(PetDocumentUpdateModel model)
        {
            var petDocumentRepo = uow.GetService<IPetDocumentRepository>();
            var petDocument = petDocumentRepo.Get().FirstOrDefault(s => s.PetDocumentId.Equals(model.PetDocumentId));
            if (petDocument != null)
            {
                petDocument = petDocumentRepo.Edit(petDocument, model);
                uow.saveChanges();
                return true;
            }
            return false;
        }
    }
}
