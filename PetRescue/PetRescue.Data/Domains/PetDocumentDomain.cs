using PetRescue.Data.Models;
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
        public bool Edit(PetDocumentUpdateModel model, Guid insertedBy)
        {
            var petDocumentRepo = uow.GetService<IPetDocumentRepository>();
            var petProfileRepo = uow.GetService<IPetProfileRepository>();
            var petDocument = petDocumentRepo.Get().FirstOrDefault(s => s.PetDocumentId.Equals(model.PetDocumentId));
            var context = uow.GetService<PetRescueContext>();
            if (petDocument != null)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        petDocument = petDocumentRepo.Edit(petDocument, model);
                        if(model.Pets != null)
                        {
                            foreach (var pet in model.Pets)
                            {
                                petProfileRepo.CreatePetProfile(pet, insertedBy, petDocument.CenterId);
                            }
                        }
                        
                        transaction.Commit();
                    }catch(Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }  
                uow.saveChanges();
                return true;
            }
            return false;
        }
    }
}
