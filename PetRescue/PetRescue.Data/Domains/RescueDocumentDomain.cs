using FirebaseAdmin.Messaging;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetRescue.Data.Domains
{
    public class RescueDocumentDomain : BaseDomain
    {
        public RescueDocumentDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public List<RescueDocumentModel> GetListRescueDocumentByCenterId(Guid centerId)
        {
            var rescueDocumentRepo = uow.GetService<IRescueDocumentRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var rescueDocuments = rescueDocumentRepo.Get().Where(s => s.CenterId.Equals(centerId));
            var result = new List<RescueDocumentModel>();
            foreach(var rescueDocument in rescueDocuments)
            {
                var currentUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(rescueDocument.FinderForm.InsertedBy));
                var finderForm = new FinderFormViewModel
                {
                    FinderDate = rescueDocument.FinderForm.InsertedAt,
                    FinderDescription = rescueDocument.FinderForm.FinderDescription,
                    FinderImageUrl = rescueDocument.FinderForm.FinderFormImgUrl,
                    FinderName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName,
                    Lat = rescueDocument.FinderForm.Lat,
                    Lng = rescueDocument.FinderForm.Lng,
                };
                currentUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(rescueDocument.PickerForm.InsertedBy));
                var pickerForm = new PickerFormViewModel
                {
                    PickerDate = rescueDocument.PickerForm.InsertedAt,
                    PickerDescription = rescueDocument.PickerForm.PickerDescription,
                    PickerImageUrl = rescueDocument.PickerForm.PickerImageUrl,
                    PickerName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName,
                };
                result.Add(new RescueDocumentModel
                {
                    FinderForm = finderForm,
                    PetDocumentId = rescueDocument.RescueDocumentId,
                    PickerForm = pickerForm,
                    PetDocumentStatus = rescueDocument.RescueDocumentStatus
                });
            }
            return result;
        }
        public bool Edit(RescueDocumentUpdateModel model, Guid insertedBy)
        {
            var rescueDocumentRepo = uow.GetService<IRescueDocumentRepository>();
            var petProfileRepo = uow.GetService<IPetProfileRepository>();
            var rescueDocument = rescueDocumentRepo.Get().FirstOrDefault(s => s.RescueDocumentStatus.Equals(model.PetDocumentId));
            var context = uow.GetService<PetRescueContext>();
            if (rescueDocument != null)
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        rescueDocument = rescueDocumentRepo.Edit(rescueDocument, model);
                        if(model.Pets != null)
                        {
                            foreach (var pet in model.Pets)
                            {
                                petProfileRepo.CreatePetProfile(pet, insertedBy, rescueDocument.CenterId);
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
        public RescueDocumentModel GetRescueDocumentByRescueDocumentId (Guid rescueDocumentId)
        {
            var rescueDocumentRepo = uow.GetService<IRescueDocumentRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var rescueDocument = rescueDocumentRepo.Get().FirstOrDefault(s => s.RescueDocumentId.Equals(rescueDocumentId));
            var result = new RescueDocumentModel();
            if(rescueDocument != null)
            {
                var currentUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(rescueDocument.FinderForm.InsertedBy));
                var finderForm = new FinderFormViewModel
                {
                    FinderDate = rescueDocument.FinderForm.InsertedAt,
                    FinderDescription = rescueDocument.FinderForm.FinderDescription,
                    FinderImageUrl = rescueDocument.FinderForm.FinderFormImgUrl,
                    FinderName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName,
                    Lat = rescueDocument.FinderForm.Lat,
                    Lng = rescueDocument.FinderForm.Lng,
                };
                currentUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(rescueDocument.PickerForm.InsertedBy));
                var pickerForm = new PickerFormViewModel
                {
                    PickerDate = rescueDocument.PickerForm.InsertedAt,
                    PickerDescription = rescueDocument.PickerForm.PickerDescription,
                    PickerImageUrl = rescueDocument.PickerForm.PickerImageUrl,
                    PickerName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName,
                };
                result.FinderForm = finderForm;
                result.PickerForm = pickerForm;
                result.PetDocumentStatus = rescueDocument.RescueDocumentStatus;
                result.PetDocumentId = rescueDocument.RescueDocumentId;
            }
            return result;
        }
        public List<PetProfileModel> GetListPetProfileByRescueDocumentId (Guid rescueDocumentId)
        {
            var petProfileRepo = uow.GetService<IPetProfileRepository>();
            var result = new List<PetProfileModel>();
            var pets = petProfileRepo.Get().Where(s => s.RescueDocumentId.Equals(rescueDocumentId));
            foreach(var pet in pets)
            {
                result.Add(new PetProfileModel {
                PetProfileId = pet.PetProfileId,
                PetBreedName = pet.PetBreed.PetBreedName,
                PetFurColorName = pet.PetFurColor.PetFurColorName,
                PetAge = pet.PetAge,
                PetGender = pet.PetGender,
                PetStatus = pet.PetStatus,
                PetImgUrl = pet.PetImgUrl,
                PetName = pet.PetName
                });
            }
            return result;
        }
        public bool CreateRescueDocument(RescueDocumentCreateModel model, Guid centerId) 
        {
            var rescueDocumentRepo = uow.GetService<IRescueDocumentRepository>();
            var result = rescueDocumentRepo.Create(model, centerId);
            if(result != null)
            {
                uow.saveChanges();
                return true;
            }
            return false;
        }
    }
}
