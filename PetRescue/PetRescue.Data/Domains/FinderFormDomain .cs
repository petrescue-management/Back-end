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
        public FinderFormDetailModel GetFinderFormById(Guid id)
        {
            
            var finderForm = uow.GetService<IFinderFormRepository>().GetFinderFormById(id);
            var user = uow.GetService<IUserRepository>().GetUserById(finderForm.InsertedBy);
            var result = new FinderFormDetailModel
            {
                FinderDate = finderForm.InsertedAt,
                FinderDescription = finderForm.FinderDescription,
                FinderFormId = finderForm.FinderFormId,
                FinderFormStatus = finderForm.FinderFormStatus,
                FinderImageUrl = finderForm.FinderFormImgUrl,
                FinderName = user.LastName + " " + user.FirstName,
                Lat = finderForm.Lat,
                Lng = finderForm.Lng,
                PetAttribute = finderForm.PetAttribute,
                Phone = finderForm.Phone,
                FinderFormVidUrl = finderForm.FinderFormVideoUrl,
                InsertedBy = finderForm.InsertedBy
            };
            return result;
        }
        #endregion

        #region UPDATE STATUS
        public async Task<FinderFormModel> UpdateFinderFormStatusAsync(UpdateStatusModel model, Guid updatedBy, string path)
        {
            var finderForm = uow.GetService<IFinderFormRepository>().UpdateFinderFormStatus(model, updatedBy);
            
            uow.saveChanges();

            if (model.Status == FinderFormStatusConst.RESCUING || model.Status == FinderFormStatusConst.CANCELED)
            {
                if(model.Status == FinderFormStatusConst.RESCUING)
                {
                    var centerId = uow.GetService<IWorkingHistoryRepository>().Get().FirstOrDefault(s=>s.UserId.Equals(updatedBy) && s.IsActive && s.RoleName.Equals(RoleConstant.VOLUNTEER)).CenterId;
                    await uow.GetService<NotificationTokenDomain>().NotificationForManager(path,(Guid) centerId, new Message 
                    {
                        Notification = new Notification
                        {
                            Title = NotificationTitleHelper.VOLUNTEER_APPROVE_PICKER_TITLE,
                            Body = NotificationBodyHelper.VOLUNTEER_APPROVE_PICKER_BODY
                        }
                    });
                    await uow.GetService<NotificationTokenDomain>().NotificationForUser(path, finderForm.InsertedBy, ApplicationNameHelper.USER_APP, new Message
                    {
                        Notification = new Notification
                        {
                            Title = NotificationTitleHelper.RESCUE_HAVE_VOLUNTEER_APPROVE_PICKED_TITLE,
                            Body = NotificationBodyHelper.RESCUE_HAVE_VOLUNTEER_APPROVE_PICKED_BODY
                        }
                    });
                }
                string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "NotificationToVolunteers.json");

                string fileJson = File.ReadAllText(FILEPATH);
                if (fileJson != null)
                {

                    var objJson = JObject.Parse(fileJson);

                    var notiArrary = objJson.GetValue("Notifications") as JArray;
                    if (notiArrary.Count != 0)
                    {
                        foreach (var noti in notiArrary.Children().ToList())
                        {

                            if (Guid.Parse(noti["FinderFormId"].Value<string>()).Equals(model.Id))
                            {
                                notiArrary.Remove(noti);

                                if (notiArrary.Count == 0)
                                    notiArrary = new JArray();

                                string output = Newtonsoft.Json.JsonConvert.SerializeObject(objJson, Newtonsoft.Json.Formatting.Indented);
                                File.WriteAllText(FILEPATH, output);
                            }

                        }
                    }
                }

                if (model.Status == FinderFormStatusConst.CANCELED)
                {
                    await uow.GetService<NotificationTokenDomain>().NotificationForUserWhenFinderFormDelete(path, finderForm.InsertedBy,
                   ApplicationNameHelper.USER_APP);
                }
            }
            else if (model.Status == FinderFormStatusConst.ARRIVED)
            {
                await uow.GetService<NotificationTokenDomain>().NotificationForUser(path, finderForm.InsertedBy, ApplicationNameHelper.USER_APP, new Message
                {
                    Notification = new Notification
                    {
                        Title = NotificationTitleHelper.ARRIVED_RESCUE_PET_TITLE,
                        Body = NotificationBodyHelper.ARRIVED_RESCUE_PET_BODY
                    }
                });
            }else if(model.Status == FinderFormStatusConst.DONE)
            {
                await uow.GetService<NotificationTokenDomain>().NotificationForUser(path, finderForm.InsertedBy, ApplicationNameHelper.USER_APP,
                new Message
                {
                    Notification = new Notification
                    {
                        Title = NotificationTitleHelper.DONE_RESCUE_PET_TITLE,
                        Body = NotificationBodyHelper.DONE_RESCUE_PET_BODY
                    }
                });
            }
            return finderForm;
        }
        public async Task<object> CancelFinderForm(CancelViewModel model,Guid updatedBy, List<string> roleName, string path)
        {
            var finderForm = uow.GetService<IFinderFormRepository>().CancelFinderForm(model, updatedBy);
            if (finderForm != null)
            {
                if (roleName != null)
                {
                    if (roleName.Contains(RoleConstant.VOLUNTEER)) 
                    {
                        await uow.GetService<NotificationTokenDomain>().NotificationForUser(path, finderForm.InsertedBy, ApplicationNameHelper.USER_APP, new Message
                        {
                            Notification = new Notification
                            {
                                Title = NotificationTitleHelper.VOLUNTEER_REJECT_FINDER_FOM_TITLE,
                                Body = NotificationBodyHelper.VOLUNTEER_REJECT_FINDER_FOM_BODY
                            }
                        });
                    }
                }
                uow.saveChanges();
                return finderForm;
            }
            return null;
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
                = new NotificationToVolunteers
                {
                    FinderFormId = finderForm.FinderFormId,
                    InsertedAt = currentTime,
                    InsertedBy = insertedBy,
                    Path = path
                };

            var serialObject = JsonConvert.SerializeObject(newJson);

            string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "NotificationToVolunteers.json");

            string fileJson = File.ReadAllText(FILEPATH);

            var objJson = JObject.Parse(fileJson);

            var notiArrary = objJson.GetValue("Notifications") as JArray;

            var newNoti = JObject.Parse(serialObject);

            if (notiArrary.Count == 0)
                notiArrary = new JArray();
            notiArrary.Add(newNoti);

            objJson["Notifications"] = notiArrary;
            string output = Newtonsoft.Json.JsonConvert.SerializeObject(objJson, Newtonsoft.Json.Formatting.Indented);

            File.WriteAllText(FILEPATH, output);

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
            if (uow.GetService<IFinderFormRepository>().GetFinderFormById(finderFormId).FinderFormStatus == FinderFormStatusConst.PROCESSING)
            {
                var centerService = uow.GetService<CenterDomain>();
                var records = centerService.GetListCenter().Select(c => c.CenterId.ToString()).ToList();
                uow.GetService<NotificationTokenDomain>().NotificationForListVolunteerOfCenter(path, records);
            }
        }

        public void DestroyNotification(Guid finderFormId, Guid insertedBy, string path)
        {
            if (uow.GetService<IFinderFormRepository>().GetFinderFormById(finderFormId).FinderFormStatus == FinderFormStatusConst.PROCESSING)
            {
                var finderForm = UpdateFinderFormStatusAsync(new UpdateStatusModel
                {
                    Id = finderFormId,
                    Status = FinderFormStatusConst.CANCELED
                }, Guid.Empty, path);
                uow.saveChanges();
            }
        }

        #endregion
        public List<FinderFormDetailModel> GetAllListFinderForm()
            {
                var userRepo = uow.GetService<IUserRepository>();
                var finderFormRepo = uow.GetService<IFinderFormRepository>();
                var result = new List<FinderFormDetailModel>();
                var finderForms = finderFormRepo.Get().Where(s => s.FinderFormStatus == FinderFormStatusConst.PROCESSING);
                foreach (var finderForm in finderForms)
                {
                    var finderUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finderForm.InsertedBy));
                    result.Add(new FinderFormDetailModel
                    {
                        FinderDate = finderForm.InsertedAt,
                        FinderDescription = finderForm.FinderDescription,
                        FinderFormId = finderForm.FinderFormId,
                        FinderFormStatus = finderForm.FinderFormStatus,
                        FinderImageUrl = finderForm.FinderFormImgUrl,
                        FinderName = finderUser.UserProfile.LastName + " " + finderUser.UserProfile.FirstName,
                        Lat = finderForm.Lat,
                        Lng = finderForm.Lng,
                        PetAttribute = finderForm.PetAttribute,
                        Phone = finderForm.Phone,
                        FinderFormVidUrl = finderForm.FinderFormVidUrl,
                        InsertedBy = finderForm.InsertedBy
                    });
                }
                return result;
            }
        public List<FinderFormDetailModel> GetListByUserId(Guid userId)
        {
            var finderFormRepo = uow.GetService<IFinderFormRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var finderForms = finderFormRepo.Get().Where(s => s.InsertedBy.Equals(userId));
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
                    FinderName = user.UserProfile.LastName + " " + user.UserProfile.FirstName,
                    Lat = finderForm.Lat,
                    Lng = finderForm.Lng,
                    PetAttribute = finderForm.PetAttribute,
                    Phone = finderForm.Phone,
                    FinderFormVidUrl = finderForm.FinderFormVidUrl,
                    InsertedBy = finderForm.InsertedBy
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
                var user = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finderForm.InsertedBy));
                result.Add(new FinderFormDetailModel
                {
                    FinderDate = finderForm.InsertedAt,
                    FinderDescription = finderForm.FinderDescription,
                    FinderFormId = finderForm.FinderFormId,
                    FinderFormStatus = finderForm.FinderFormStatus,
                    FinderImageUrl = finderForm.FinderFormImgUrl,
                    FinderName = user.UserProfile.LastName + " " + user.UserProfile.FirstName,
                    Lat = finderForm.Lat,
                    Lng = finderForm.Lng,
                    PetAttribute = finderForm.PetAttribute,
                    Phone = finderForm.Phone,
                    FinderFormVidUrl = finderForm.FinderFormVidUrl,
                    InsertedBy = finderForm.InsertedBy
                });
            }
            return result;
        }
        public List<FinderFormViewModel2> GetListFinderFormFinishByUserId(Guid userId)
        {
            var finderFormRepo = uow.GetService<IFinderFormRepository>();
            var userRepo = uow.GetService<IUserRepository>();
            var result = new List<FinderFormViewModel2>();
            var finders = finderFormRepo.Get().Where(s => s.UpdatedBy.Equals(userId) && s.FinderFormStatus == FinderFormStatusConst.DONE);
            foreach (var finder in finders)
            {
                var temp = new FinderFormViewModel2
                {
                    FinderDate = finder.InsertedAt,
                    FinderFormStatus = finder.FinderFormStatus,
                    FinderDescription = finder.FinderDescription,
                    FinderFormId = finder.FinderFormId,
                    FinderImageUrl = finder.FinderFormImgUrl,
                    FinderFormVidUrl = finder.FinderFormVidUrl,
                    PetAttribute = finder.PetAttribute,
                    PickerDate = finder.RescueDocument.PickerForm.InsertedAt,
                    PickerFormDescription = finder.RescueDocument.PickerForm.PickerDescription,
                    PickerFormId = finder.RescueDocument.PickerForm.PickerFormId,
                    PickerFormImg = finder.RescueDocument.PickerForm.PickerImageUrl,

                };
                var currentUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finder.InsertedBy));
                temp.FinderName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName;
                currentUser = userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finder.UpdatedBy));
                temp.PickerName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName;
                result.Add(temp);
            }
            return result;
        }

    }
        
    }
