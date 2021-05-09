using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRescueDocumentRepository _rescueDocumentRepo;
        private readonly IUserRepository _userRepo;
        private readonly IPetProfileRepository _petProfileRepo;
        private readonly DbContext _context;
        public RescueDocumentDomain(IUnitOfWork uow, 
            IRescueDocumentRepository rescueDocumentRepo, 
            IUserRepository userRepo, 
            IPetProfileRepository petProfileRepo, 
            DbContext context) : base(uow)
        {
            this._rescueDocumentRepo = rescueDocumentRepo;
            this._userRepo = userRepo;
            this._petProfileRepo = petProfileRepo;
            this._context = context;
        }
        public object GetListRescueDocumentByCenterId(Guid centerId, int page, int limit)
        {

            var rescueDocuments = _rescueDocumentRepo.Get().Where(s => s.CenterId.Equals(centerId));
            var total = 0;
            if (limit == 0)
            {
                limit = 1;
            }
            if (limit > -1)
            {
                total = rescueDocuments.Count() / limit;
            }
            rescueDocuments = rescueDocuments.OrderByDescending(s => s.PickerForm.InsertedAt);
            if (limit > -1 && page >= 0)
            {
                rescueDocuments = rescueDocuments.Skip(page * limit).Take(limit);
            }
            var listRescueDocuments = new List<RescueDocumentModel>();
            foreach (var rescueDocument in rescueDocuments)
            {
                var currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(rescueDocument.FinderForm.InsertedBy));
                var finderForm = new FinderFormViewModel
                {
                    FinderDate = rescueDocument.FinderForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    FinderDescription = rescueDocument.FinderForm.Description,
                    FinderImageUrl = rescueDocument.FinderForm.FinderFormImgUrl,
                    FinderName = currentUser.UserNavigation.LastName + " " + currentUser.UserNavigation.FirstName,
                    Lat = rescueDocument.FinderForm.Lat,
                    Lng = rescueDocument.FinderForm.Lng,
                    FinderFormVidUrl = rescueDocument.FinderForm.FinderFormVidUrl
                };
                currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(rescueDocument.PickerForm.InsertedBy));
                var pickerForm = new PickerFormViewModel
                {
                    PickerDate = rescueDocument.PickerForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    PickerDescription = rescueDocument.PickerForm.Description,
                    PickerImageUrl = rescueDocument.PickerForm.Description,
                    PickerName = currentUser.UserNavigation.LastName + " " + currentUser.UserNavigation.FirstName,
                };
                listRescueDocuments.Add(new RescueDocumentModel
                {
                    FinderForm = finderForm,
                    PetDocumentId = rescueDocument.RescueDocumentId,
                    PickerForm = pickerForm,
                    PetDocumentStatus = rescueDocument.RescueDocumentStatus
                });
            }
            var result = new Dictionary<string, object>()
            {
                ["totalPages"] = total,
                ["result"] = listRescueDocuments
            };
            return result;
        }
        public bool Edit(RescueDocumentUpdateModel model, Guid insertedBy)
        {
            var rescueDocument = _rescueDocumentRepo.Get().FirstOrDefault(s => s.RescueDocumentId.Equals(model.PetDocumentId));
            if (rescueDocument != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        rescueDocument = _rescueDocumentRepo.Edit(rescueDocument, model);
                        if (model.Pets != null)
                        {
                            foreach (var pet in model.Pets)
                            {
                                _petProfileRepo.CreatePetProfile(pet, insertedBy, (Guid)rescueDocument.CenterId);
                            }
                        }
                        transaction.Commit();
                    } catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
                _uow.SaveChanges();
                return true;
            }
            return false;
        }
        public RescueDocumentModel GetRescueDocumentByRescueDocumentId(Guid rescueDocumentId)
        {
            var rescueDocument = _rescueDocumentRepo.Get().FirstOrDefault(s => s.RescueDocumentId.Equals(rescueDocumentId));
            var result = new RescueDocumentModel();
            if (rescueDocument != null)
            {
                var currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(rescueDocument.FinderForm.InsertedBy));
                var finderForm = new FinderFormViewModel
                {
                    FinderDate = rescueDocument.FinderForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    FinderDescription = rescueDocument.FinderForm.Description,
                    FinderImageUrl = rescueDocument.FinderForm.FinderFormImgUrl,
                    FinderName = currentUser.UserNavigation.LastName + " " + currentUser.UserNavigation.FirstName,
                    Lat = rescueDocument.FinderForm.Lat,
                    Lng = rescueDocument.FinderForm.Lng,
                    FinderFormVidUrl = rescueDocument.FinderForm.FinderFormVidUrl
                };
                currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(rescueDocument.PickerForm.InsertedBy));
                var pickerForm = new PickerFormViewModel
                {
                    PickerDate = rescueDocument.PickerForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    PickerDescription = rescueDocument.PickerForm.Description,
                    PickerImageUrl = rescueDocument.PickerForm.PickerFormImgUrl,
                    PickerName = currentUser.UserNavigation.LastName + " " + currentUser.UserNavigation.FirstName,
                };
                result.FinderForm = finderForm;
                result.PickerForm = pickerForm;
                result.PetDocumentStatus = rescueDocument.RescueDocumentStatus;
                result.PetDocumentId = rescueDocument.RescueDocumentId;
            }
            return result;
        }
        public List<PetProfileModel> GetListPetProfileByRescueDocumentId(Guid rescueDocumentId)
        {
            var result = new List<PetProfileModel>();
            var pets = _petProfileRepo.Get().Where(s => s.RescueDocumentId.Equals(rescueDocumentId));
            foreach (var pet in pets)
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
            var result = _rescueDocumentRepo.Create(model, centerId);
            if (IsValid(model.FinderFormId, model.PickerFormId))
            {
                if (result != null)
                {
                    _uow.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        private bool IsValid(Guid finderFormId, Guid pickerFormId)
        {
            var result = _rescueDocumentRepo.Get().FirstOrDefault(s => s.FinderFormId.Equals(finderFormId) || s.PickerFormId.Equals(pickerFormId));
            if (result != null) return false;
            return true;
        }
        public object GetLastedRescueDocument(Guid centerId)
        {
            var listDoc = _rescueDocumentRepo.Get()
                .Where(s => s.CenterId.Equals(centerId)).Skip(0)
                .Take(5)
                .OrderByDescending(s => s.PickerForm.InsertedAt)
                .Select(s => new
                {
                    time = s.PickerForm.InsertedAt
                });

            return listDoc;
        }
    }
}
