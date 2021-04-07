using FirebaseAdmin.Messaging;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Extensions;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PetRescue.Data.Domains
{
    public class FinderFormDomain : BaseDomain
    {
        public FinderFormDomain(IUnitOfWork uow) : base(uow)
        {
        }

        #region SEARCH
        public SearchReturnModel SearchFinderForm(SearchModel model)
        {
            var records = uow.GetService<IFinderFormRepository>().Get().AsQueryable();


            if (model.Status != 0)
                records = records.Where(f => f.FinderFormStatus.Equals(model.Status));

            List<FinderFormModel> result = records
                .Skip((model.PageIndex - 1) * model.PageSize)
                .Take(model.PageSize)
                .Select(f => new FinderFormModel
                {
                    FinderFormId = f.FinderFormId,
                    Lat = f.Lat,
                    Lng = f.Lng,
                    FinderFormImgUrl = f.FinderFormImgUrl,
                    PetAttribute = f.PetAttribute,
                    FinderDescription = f.FinderDescription,
                    FinderFormStatus = f.FinderFormStatus,
                    Phone = f.Phone,
                    InsertedBy = f.InsertedBy,
                    InsertedAt = f.InsertedAt,
                }).ToList();
            return new SearchReturnModel
            {
                TotalCount = records.Count(),
                Result = result
            };
        }
        #endregion

        #region GET BY ID
        public FinderFormModel GetFinderFormById(Guid id)
        {
            var finderForm = uow.GetService<IFinderFormRepository>().GetFinderFormById(id);
            return finderForm;
        }
        #endregion

        #region UPDATE STATUS
        public async Task<FinderFormModel> UpdateFinderFormStatus(UpdateStatusModel model, Guid updatedBy)
        {
            var finderForm = uow.GetService<IFinderFormRepository>().UpdateFinderFormStatus(model, updatedBy);
            uow.saveChanges();
            return finderForm;
        }
        #endregion

        #region CREATE
        public FinderFormModel CreateFinderForm(CreateFinderFormModel model, Guid insertedBy, string path)
        {
            DateTime currentTime = DateTime.UtcNow;

            var finderFormService = uow.GetService<IFinderFormRepository>();

            var finderForm = finderFormService.CreateFinderForm(model, insertedBy);
            uow.saveChanges();
            //tạo object lưu xuống json
            var newJson
                = new NotificationToVolunteers {
                    FinderFormId = finderForm.FinderFormId,
                    CurrentTime = currentTime,
                    InsertedBy = insertedBy,
                    path = path
                };

            var serialObject = JsonConvert.SerializeObject(newJson);

            string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "NotificationToVolunteers.json");

            string fileJson = File.ReadAllText(FILEPATH);

            var jsonObj = JObject.Parse(fileJson);

            var notiArrary = jsonObj.GetValue("notifications") as JArray;

            var newNoti = JObject.Parse(serialObject);

            if (notiArrary.Count == 0)
                notiArrary = new JArray();

            notiArrary.Add(newNoti);

            jsonObj["notifications"] = notiArrary;
            string newJsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj,
                                   Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(FILEPATH, newJsonResult);

            var centerService = uow.GetService<CenterDomain>();
            var centers = centerService.GetListCenterLocation();
            var googleMapExtension = new GoogleMapExtensions();
            var location = model.Lat + ", " + model.Lng;
            var records = googleMapExtension.FindListShortestCenter(location, centers);
            var listCenterId = new List<string>();

            if (records.Count != 0)
            {
                if (records.Count >= 2)
                {
                    listCenterId.Add(records[0].CenterId);
                    listCenterId.Add(records[1].CenterId);
                }
                else
                {
                    listCenterId.Add(records[0].CenterId);
                }
            }
            uow.GetService<NotificationTokenDomain>().NotificationForListVolunteerOfCenter(path, listCenterId);
            uow.saveChanges();
            return finderForm;
        }

        public void ReNotification(Guid finderFormId, string path)
        {
            if (uow.GetService<IFinderFormRepository>().GetFinderFormById(finderFormId).FinderFormStatus == 1)
            {
                var centerService = uow.GetService<CenterDomain>();
                var records = centerService.GetListCenter().Select(c => c.CenterId.ToString()).ToList();
                uow.GetService<NotificationTokenDomain>().NotificationForListVolunteerOfCenter(path, records);
                uow.saveChanges();
            }
        }

        public void DestroyNotification(Guid finderFormId, Guid insertedBy)
        {
            if (uow.GetService<IFinderFormRepository>().GetFinderFormById(finderFormId).FinderFormStatus == 1)
            {
                var finderForm = uow.GetService<IFinderFormRepository>().UpdateFinderFormStatus(
                    new UpdateStatusModel {
                        Id = finderFormId,
                        Status = 3
                    },
                     Guid.Empty);
                uow.GetService<NotificationTokenDomain>().DeleteNotificationByUserIdAndApplicationName(insertedBy, ApplicationNameHelper.USER_APP);
                uow.saveChanges();
            }
        }

        #endregion

        public List<FinderFormDetailModel> GetAllListFinderForm()
        {
            var userRepo = uow.GetService<IUserRepository>();
            var finderFormRepo = uow.GetService<IFinderFormRepository>();
            var result = new List<FinderFormDetailModel>();
            var finderForms = finderFormRepo.Get().Where(s => s.FinderFormStatus == FinderFormStatus.PROCESSING);
            foreach(var finderForm in finderForms)
            {
                var finderUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finderForm.InsertedBy));
                result.Add(new FinderFormDetailModel
                {
                    FinderDate = finderForm.InsertedAt,
                    FinderDescription = finderForm.FinderDescription,
                    FinderFormId =finderForm.FinderFormId,
                    FinderFormStatus = finderForm.FinderFormStatus,
                    FinderImageUrl = finderForm.FinderFormImgUrl,
                    FinderName = finderUser.UserProfile.FirstName + " " + finderUser.UserProfile.LastName,
                    Lat = finderForm.Lat,
                    Lng = finderForm.Lng,
                    PetAttribute = finderForm.PetAttribute,
                    phone = finderForm.Phone,
                });
            }
            return result;
        }
        public List<FinderFormDetailModel> GetListByUserId(Guid userId) {
            var finderFormRepo = uow.GetService<IFinderFormRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var finderForms = finderFormRepo.Get().Where(s =>s.InsertedBy.Equals(userId));
            var result = new List<FinderFormDetailModel>();
            foreach (var finderForm in finderForms)
            {
                var user = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(userId));
                result.Add(new FinderFormDetailModel
                {
                    FinderDate = finderForm.InsertedAt,
                    FinderDescription = finderForm.FinderDescription,
                    FinderFormId = finderForm.FinderFormId,
                    FinderFormStatus = finderForm.FinderFormStatus,
                    FinderImageUrl = finderForm.FinderFormImgUrl,
                    FinderName = user.UserProfile.FirstName + " " + user.UserProfile.LastName,
                    Lat = finderForm.Lat,
                    Lng = finderForm.Lng,
                    PetAttribute = finderForm.PetAttribute,
                    phone = finderForm.Phone,
                });
            }
            return result;
        }
        public List<FinderFormDetailModel> GetListByStatus(Guid updatedBy, int status)
        {
            var finderFormRepo = uow.GetService<IFinderFormRepository>();
            var finderForms = finderFormRepo.Get().Where(s => s.UpdatedBy.Equals(updatedBy) && s.FinderFormStatus == status);
            var result = new List<FinderFormDetailModel>();
            var userRepo = uow.GetService<IUserRepository>();
            foreach (var finderForm in finderForms)
            {
                var user = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(updatedBy));
                result.Add(new FinderFormDetailModel
                {
                    FinderDate = finderForm.InsertedAt,
                    FinderDescription = finderForm.FinderDescription,
                    FinderFormId = finderForm.FinderFormId,
                    FinderFormStatus = finderForm.FinderFormStatus,
                    FinderImageUrl = finderForm.FinderFormImgUrl,
                    FinderName = user.UserProfile.FirstName + " " + user.UserProfile.LastName,
                    Lat = finderForm.Lat,
                    Lng = finderForm.Lng,
                    PetAttribute = finderForm.PetAttribute,
                    phone = finderForm.Phone,
                });
            }
            return result;
        }
    }
}
