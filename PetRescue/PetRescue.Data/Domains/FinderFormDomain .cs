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
        public async Task<object> UpdateFinderFormStatusAsync(UpdateStatusModel model, Guid updatedBy, string path)
        {
            if (IsTaken(model, updatedBy))
            {

                TransactionOptions topt = new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.RepeatableRead
                };
                var finderForm = _finderFormRepo.UpdateFinderFormStatus(model, updatedBy);
                using (var tran = new TransactionScope(TransactionScopeOption.Required, topt))
                {
                    
                    _uow.SaveChanges();
                    tran.Complete();
                }
                
                if (model.Status == FinderFormStatusConst.RESCUING || model.Status == FinderFormStatusConst.CANCELED)
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

                    if (model.Status == FinderFormStatusConst.CANCELED)
                    {
                        await _uow.GetService<NotificationTokenDomain>().NotificationForUserWhenFinderFormDelete(path, (Guid)finderForm.InsertedBy,
                       ApplicationNameHelper.USER_APP);
                    }
                }
                //else if (model.Status == FinderFormStatusConst.ARRIVED)
                //{
                    
                //    await _uow.GetService<NotificationTokenDomain>().NotificationForUser(path, (Guid)finderForm.InsertedBy, ApplicationNameHelper.USER_APP, new Message
                //    {
                //        Notification = new Notification
                //        {
                //            Title = NotificationTitleHelper.ARRIVED_RESCUE_PET_TITLE,
                //            Body = NotificationBodyHelper.ARRIVED_RESCUE_PET_BODY
                //        }
                //    });
                //    await _uow.GetService<NotificationTokenDomain>().NotificationForManager(path, centerId,
                //    new Message
                //    {
                //        Notification = new Notification
                //        {
                //            Title = NotificationTitleHelper.VOLUNTEER_ARRVING_TITLE,
                //            Body = NotificationBodyHelper.VOLUNTEER_ARRVING_BODY
                //        }
                //    });
                //}
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
            _uow.SaveChanges();

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
            var centers = _uow.GetService<CenterDomain>().GetListCenterLocation();
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
            await _uow.GetService<NotificationTokenDomain>().NotificationForListVolunteerOfCenter(path, listCenterId);
            _uow.SaveChanges();
            return finderForm;
        }

        public async void ReNotification(Guid finderFormId, string path)
        {
            if (_finderFormRepo.GetFinderFormById(finderFormId).FinderFormStatus == FinderFormStatusConst.PROCESSING)
            {
                var records = _uow.GetService<CenterDomain>().GetListCenter().Select(c => c.CenterId.ToString()).ToList();
                await _uow.GetService<NotificationTokenDomain>().NotificationForListVolunteerOfCenter(path, records);
            }
        }

        public async void DestroyNotification(Guid finderFormId, Guid insertedBy, string path)
        {
            if (_finderFormRepo.GetFinderFormById(finderFormId).FinderFormStatus == FinderFormStatusConst.PROCESSING)
            {
                await UpdateFinderFormStatusAsync(new UpdateStatusModel
                {
                    Id = finderFormId,
                    Status = FinderFormStatusConst.CANCELED
                }, Guid.Empty, path);
                _uow.SaveChanges();
            }
        }

        #endregion
        public List<FinderFormDetailModel> GetAllListFinderForm()
        {
            
            var result = new List<FinderFormDetailModel>();
            var finderForms = _finderFormRepo.Get().Where(s => s.FinderFormStatus == FinderFormStatusConst.PROCESSING);
            foreach (var finderForm in finderForms)
            {
                var finderUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finderForm.InsertedBy));
                result.Add(new FinderFormDetailModel
                {
                    FinderDate = finderForm.InsertedAt?.AddHours(ConstHelper.UTC_VIETNAM),
                    Description = finderForm.Description,
                    FinderFormId = finderForm.FinderFormId,
                    FinderFormStatus = finderForm.FinderFormStatus,
                    FinderImageUrl = finderForm.FinderFormImgUrl,
                    FinderName = finderUser.UserNavigation.LastName + " " + finderUser.UserNavigation.FirstName,
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
                    FinderName = user.UserNavigation.LastName + " " + user.UserNavigation.FirstName,
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
                    FinderName = user.UserNavigation.LastName + " " + user.UserNavigation.FirstName,
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
                temp.FinderName = currentUser.UserNavigation.LastName + " " + currentUser.UserNavigation.FirstName;
                currentUser = _userRepo.Get().FirstOrDefault(s => s.UserId.Equals(finder.UpdatedBy));
                temp.PickerName = currentUser.UserNavigation.LastName + " " + currentUser.UserNavigation.FirstName;
                result.Add(temp);
            }
            return result;
        }
        public bool IsTaken(UpdateStatusModel model, Guid updatedBy)
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
