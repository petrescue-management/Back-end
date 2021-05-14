using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
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
using System.Transactions;

namespace PetRescue.Data.Domains
{
    public class FinderFormDomain : BaseDomain
    {
        private readonly IFinderFormRepository _finderFormRepo;
        private readonly IUserRepository _userRepo;
        public FinderFormDomain(IUnitOfWork uow,
            IFinderFormRepository finderFormRepo,
            IUserRepository userRepo) : base(uow)
        {
            this._finderFormRepo = finderFormRepo;
            this._userRepo = userRepo;
        }
        #region SEARCH
        public SearchReturnModel SearchFinderForm(SearchModel model)
        {
            var records = _finderFormRepo.Get().AsQueryable();
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
                    Description = f.Description,
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

            var finderForm = _finderFormRepo.GetFinderFormById(id);
            var user = _userRepo.GetUserById((Guid)finderForm.InsertedBy);
            var result = new FinderFormDetailModel
            {
                FinderDate = finderForm.InsertedAt,
                Description = finderForm.Description,
                FinderFormId = finderForm.FinderFormId,
                FinderFormStatus = finderForm.FinderFormStatus,
                FinderImageUrl = finderForm.FinderFormImgUrl,
                FinderName = user.LastName + " " + user.FirstName,
                Lat = finderForm.Lat,
                Lng = finderForm.Lng,
                PetAttribute = finderForm.PetAttribute,
                Phone = finderForm.Phone,
                FinderFormVidUrl = finderForm.FinderFormVideoUrl,
                InsertedBy = finderForm.InsertedBy,
                CanceledReason = finderForm.CanceledReason
                
            };
            return result;
        }
        #endregion

        #region UPDATE STATUS
        public async Task<FinderFormModel> UpdateFinderFormStatusAsync(UpdateStatusFinderFormModel model, Guid updatedBy, string path)
        {
            if (IsTaken(model, updatedBy))
            {

                TransactionOptions topt = new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.RepeatableRead
                };
                var finderForm = _finderFormRepo.UpdateFinderFormStatus(new UpdateStatusModel 
                {
                    Id = model.Id,
                    Status = model.Status
                }, updatedBy);
                using (var tran = new TransactionScope(TransactionScopeOption.Required, topt))
                {
                    
                    _uow.SaveChanges();
                    tran.Complete();
                }
                
                if (model.Status == FinderFormStatusConst.RESCUING || model.Status == FinderFormStatusConst.DROPPED)
                {
                    if (model.Status == FinderFormStatusConst.RESCUING)
                    {
                        
                        await _uow.GetService<NotificationTokenDomain>().NotificationForUser(path, (Guid)finderForm.InsertedBy, ApplicationNameHelper.USER_APP, new Message
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

                    if (model.Status == FinderFormStatusConst.DROPPED)
                    {
                        await _uow.GetService<NotificationTokenDomain>().NotificationForUserWhenFinderFormDelete(path, (Guid)finderForm.InsertedBy,
                       ApplicationNameHelper.USER_APP);
                    }
                }
                else if (model.Status == FinderFormStatusConst.ARRIVED)
                {

                    await _uow.GetService<NotificationTokenDomain>().NotificationForUser(path, (Guid)finderForm.InsertedBy, ApplicationNameHelper.USER_APP, new Message
                    {
                        Notification = new Notification
                        {
                            Title = NotificationTitleHelper.ARRIVED_RESCUE_PET_TITLE,
                            Body = NotificationBodyHelper.ARRIVED_RESCUE_PET_BODY
                        }
                    });
                    await _uow.GetService<NotificationTokenDomain>().NotificationForManager(path, (Guid)model.CenterId,
                    new Message
                    {
                        Notification = new Notification
                        {
                            Title = NotificationTitleHelper.VOLUNTEER_ARRVING_TITLE,
                            Body = NotificationBodyHelper.VOLUNTEER_ARRVING_BODY
                        }
                    });
                }
                else if (model.Status == FinderFormStatusConst.DONE)
                {
                    await _uow.GetService<NotificationTokenDomain>().NotificationForUser(path, (Guid)finderForm.InsertedBy, ApplicationNameHelper.USER_APP,
                            new Message
                            {
                                Notification = new Notification
                                {
                                    Title = NotificationTitleHelper.DONE_RESCUE_PET_TITLE,
                                    Body = NotificationBodyHelper.DONE_RESCUE_PET_BODY
                                }
                            });
                }
                return new FinderFormModel
                {
                    FinderFormId = finderForm.FinderFormId,
                    Lat = finderForm.Lat,
                    Lng = finderForm.Lng,
                    FinderFormImgUrl = finderForm.FinderFormImgUrl,
                    FinderFormVideoUrl = finderForm.FinderFormVidUrl,
                    CanceledReason = finderForm.DroppedReason,
                    PetAttribute = finderForm.PetAttribute,
                    Description = finderForm.Description,
                    FinderFormStatus = finderForm.FinderFormStatus,
                    Phone = finderForm.Phone,
                    InsertedBy = finderForm.InsertedBy,
                    InsertedAt = finderForm.InsertedAt,
                    UpdatedAt = finderForm.UpdatedAt,
                    UpdatedBy = finderForm.UpdatedBy,
                };
            }
            else
            {
                return null;
            }
        }
        public async Task<object> CancelFinderForm(CancelViewModel model, Guid updatedBy, List<string> roleName, string path)
        {
            var finderForm = _finderFormRepo.CancelFinderForm(model, updatedBy);
            if (finderForm != null)
            {
                if (roleName != null)
                {
                    if (roleName.Contains(RoleConstant.VOLUNTEER))
                    {
                        await _uow.GetService<NotificationTokenDomain>().NotificationForUser(path, (Guid)finderForm.InsertedBy, ApplicationNameHelper.USER_APP, new Message
                        {
                            Notification = new Notification
                            {
                                Title = NotificationTitleHelper.VOLUNTEER_REJECT_FINDER_FORM_TITLE,
                                Body = NotificationBodyHelper.VOLUNTEER_REJECT_FINDER_FORM_BODY
                            }
                        });
                    }
                }
                _uow.SaveChanges();
                return finderForm;
            }
            return null;
        }
        #endregion
        #region CREATE
        public async Task<FinderFormModel> CreateFinderForm(CreateFinderFormModel model, Guid insertedBy, string path)
        {
            DateTime currentTime = DateTime.UtcNow;
            var finderForm = _finderFormRepo.CreateFinderForm(model, insertedBy);
            if(finderForm == null)
            {
                return null;
            }
            string realPath = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "SystemParameters.json");
            var fileTxt = File.ReadAllText(realPath);
            var systemParameter = JObject.Parse(fileTxt);
            var availableDistance = double.Parse(systemParameter["NearestDistance"].Value<string>());
            var origin = model.Lat + ", " + model.Lng;
            var fileExtension = new FileExtension();
            var googleMapExtensions = new GoogleMapExtensions();
            var locations = fileExtension.GetAvailableVolunteerLocation();
            var distances = googleMapExtensions.FindDistanceVoLunteer(origin, locations);
            if (distances != null)
            {
                foreach (var distance in distances)
                {
                    if (distance.Value <= availableDistance)
                    {
                        await _uow.GetService<NotificationTokenDomain>().NotificationForUser(path, 
                            distance.UserId,
                            ApplicationNameHelper.VOLUNTEER_APP,
                            new Message 
                            {
                                Notification = new Notification
                                {
                                    Body = NotificationBodyHelper.NEW_RESCUE_NEAREST_FORM_BODY,
                                    Title = NotificationTitleHelper.NEW_RESCUE_NEAREST_FORM_TITLE
                                }
                            });
                    }
                }
            }
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
            _uow.SaveChanges();
            return finderForm;
        }

        public async void ReNotificationForOnline(Guid finderFormId, string path)
        {
            if (_finderFormRepo.GetFinderFormById(finderFormId).FinderFormStatus == FinderFormStatusConst.PROCESSING)
            {
                var records = _uow.GetService<CenterDomain>().GetListCenter().Select(c => c.CenterId.ToString()).ToList();
                await _uow.GetService<NotificationTokenDomain>().NotificationForOnlineVolunteers(path);
            }
        }

        public async void ReNotificationForAll(Guid finderFormId, string path)
        {
            if (_finderFormRepo.GetFinderFormById(finderFormId).FinderFormStatus == FinderFormStatusConst.PROCESSING)
            {
                var records = _uow.GetService<CenterDomain>().GetListCenter().Select(c => c.CenterId.ToString()).ToList();
                await _uow.GetService<NotificationTokenDomain>().NotificationForAllVolunteers(path);
            }
        }

        public async void DestroyNotification(Guid finderFormId, Guid insertedBy, string path)
        {
            if (_finderFormRepo.GetFinderFormById(finderFormId).FinderFormStatus == FinderFormStatusConst.PROCESSING)
            {
                var form = await UpdateFinderFormStatusAsync(new UpdateStatusFinderFormModel
                {
                    Id = finderFormId,
                    Status = FinderFormStatusConst.DROPPED
                }, Guid.Empty, path);
                await _uow.GetService<NotificationTokenDomain>().NotificationForUserWhenFinderFormDelete(path, (Guid)form.InsertedBy, ApplicationNameHelper.USER_APP);
                _uow.SaveChanges();
            }
        }

        #endregion
        public object GetAllListFinderForm(Guid userId)
        {
            var result = new List<FinderFormDetailModel3>();
            var finderForms = _finderFormRepo.Get().Where(s => s.FinderFormStatus == FinderFormStatusConst.PROCESSING);
            var googleMapExtension = new GoogleMapExtensions();
            var fileExtension = new FileExtension();
            var listLocation = fileExtension.GetAvailableVolunteerLocation();
            var location = new UserLocation();
            bool hasValue = listLocation.TryGetValue(userId, out location);
            if (!hasValue)
            {
                return result;
            }
            var listFinderFormLocation = new List<FinderFormLocationModel>();
            string FILEPATH =Path.Combine(Directory.GetCurrentDirectory(), "JSON", "SystemParameters.json");
            var fileJson = File.ReadAllText(FILEPATH);
            var time = JObject.Parse(fileJson);
            var nearestDistance = double.Parse(time["NearestDistance"].Value<string>());
            var ReNotiTimeForRescue = double.Parse(time["ReNotiTimeForRescue"].Value<string>());
            foreach (var finderForm in finderForms)
            {
                listFinderFormLocation.Add(new FinderFormLocationModel 
                {
                    FinderFormId = finderForm.FinderFormId,
                    Lat = finderForm.Lat,
                    Lng = finderForm.Lng
                });
            }
            var records = googleMapExtension.FindDistanceRescueRequest(location.Lat +", " + location.Long, listFinderFormLocation);
            foreach (var record in records)
            {
                var finderForm = _finderFormRepo.Get().FirstOrDefault(s => s.FinderFormId.Equals(record.FinderFormId));
                if (record.Value <= nearestDistance)
                {
                    var currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finderForm.InsertedBy));
                    result.Add(new FinderFormDetailModel3
                    {
                        CanceledReason = finderForm.DroppedReason,
                        FinderFormId = finderForm.FinderFormId,
                        Description = finderForm.Description,
                        FinderDate = finderForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                        FinderFormStatus = finderForm.FinderFormStatus,
                        FinderFormVidUrl = finderForm.FinderFormVidUrl,
                        FinderImageUrl = finderForm.FinderFormImgUrl,
                        FinderName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName,
                        InsertedBy = currentUser.UserId,
                        Lat = finderForm.Lat,
                        Lng = finderForm.Lng,
                        PetAttribute = finderForm.PetAttribute,
                        Phone = finderForm.Phone,
                        Distance = Math.Round(record.Value / 1000, 2)
                    });
                }
                else
                {
                    if(DateTime.UtcNow.Subtract((DateTime)finderForm.InsertedAt).TotalSeconds >= ReNotiTimeForRescue * 60)
                    {
                        var currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finderForm.InsertedBy));
                        result.Add(new FinderFormDetailModel3
                        {
                            CanceledReason = finderForm.DroppedReason,
                            FinderFormId = finderForm.FinderFormId,
                            Description = finderForm.Description,
                            FinderDate = finderForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                            FinderFormStatus = finderForm.FinderFormStatus,
                            FinderFormVidUrl = finderForm.FinderFormVidUrl,
                            FinderImageUrl = finderForm.FinderFormImgUrl,
                            FinderName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName,
                            InsertedBy = currentUser.UserId,
                            Lat = finderForm.Lat,
                            Lng = finderForm.Lng,
                            PetAttribute = finderForm.PetAttribute,
                            Phone = finderForm.Phone,
                            Distance = Math.Round(record.Value / 1000, 2)
                        });
                    }
                }
            }
            return result;
        }
        
        public List<FinderFormDetailModel> GetListByUserId(Guid userId)
        {
            var finderForms = _finderFormRepo.Get().Where(s => s.InsertedBy.Equals(userId));
            var result = new List<FinderFormDetailModel>();
            foreach (var finderForm in finderForms)
            {
                var user = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(userId));
                result.Add(new FinderFormDetailModel
                {
                    FinderDate = finderForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    Description = finderForm.Description,
                    FinderFormId = finderForm.FinderFormId,
                    FinderFormStatus = finderForm.FinderFormStatus,
                    FinderImageUrl = finderForm.FinderFormImgUrl,
                    FinderName = user.UserProfile.LastName + " " + user.UserProfile.FirstName,
                    Lat = finderForm.Lat,
                    Lng = finderForm.Lng,
                    PetAttribute = finderForm.PetAttribute,
                    Phone = finderForm.Phone,
                    FinderFormVidUrl = finderForm.FinderFormVidUrl,
                    CanceledReason = finderForm.DroppedReason,
                    InsertedBy = finderForm.InsertedBy

                });
            }
            return result;
        }
        public List<FinderFormDetailModel> GetListByStatus(Guid updatedBy, int status)
        {
            var finderForms = _finderFormRepo.Get().Where(s => s.UpdatedBy.Equals(updatedBy) && s.FinderFormStatus == status);
            var result = new List<FinderFormDetailModel>();
            foreach (var finderForm in finderForms)
            {
                var user = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finderForm.InsertedBy));
                result.Add(new FinderFormDetailModel
                {
                    FinderDate = finderForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    Description = finderForm.Description,
                    FinderFormId = finderForm.FinderFormId,
                    FinderFormStatus = finderForm.FinderFormStatus,
                    FinderImageUrl = finderForm.FinderFormImgUrl,
                    FinderName = user.UserProfile.LastName + " " + user.UserProfile.FirstName,
                    Lat = finderForm.Lat,
                    Lng = finderForm.Lng,
                    PetAttribute = finderForm.PetAttribute,
                    Phone = finderForm.Phone,
                    FinderFormVidUrl = finderForm.FinderFormVidUrl,
                    InsertedBy = finderForm.InsertedBy,
                    CanceledReason = finderForm.DroppedReason
                });
            }
            return result;
        }
        public List<FinderFormViewModel2> GetListFinderFormFinishByUserId(Guid userId)
        {
            var result = new List<FinderFormViewModel2>();
            var finders = _finderFormRepo.Get().Where(s => s.UpdatedBy.Equals(userId) && s.FinderFormStatus == FinderFormStatusConst.DONE);
            foreach (var finder in finders)
            {
                var temp = new FinderFormViewModel2
                {
                    FinderDate = finder.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    FinderFormStatus = finder.FinderFormStatus,
                    FinderDescription = finder.Description,
                    FinderFormId = finder.FinderFormId,
                    FinderImageUrl = finder.FinderFormImgUrl,
                    FinderFormVidUrl = finder.FinderFormVidUrl,
                    PetAttribute = finder.PetAttribute,
                    PickerDate = finder.RescueDocument.PickerForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    PickerFormDescription = finder.RescueDocument.PickerForm.Description,
                    PickerFormId = finder.RescueDocument.PickerForm.PickerFormId,
                    PickerFormImg = finder.RescueDocument.PickerForm.PickerFormImgUrl,
                    CanceledReason = finder.DroppedReason
                };
                var currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finder.InsertedBy));
                temp.FinderName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName;
                currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finder.UpdatedBy));
                temp.PickerName = currentUser.UserProfile.LastName + " " + currentUser.UserProfile.FirstName;
                result.Add(temp);
            }
            return result;
        }
        public bool IsTaken(UpdateStatusFinderFormModel model, Guid updatedBy)
        {
            var finderForm = _finderFormRepo.Get().FirstOrDefault(s => s.FinderFormId.Equals(model.Id));
            if (finderForm.UpdatedBy == null)
            {
                return true;
            }
            else
            {
                if (finderForm.UpdatedBy.Equals(updatedBy))
                {
                    return true;
                }
            }
            return false;
        }
    }

}
