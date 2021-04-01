using Microsoft.VisualBasic;
using Newtonsoft.Json;
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
                    UpdatedBy = f.UpdatedBy,
                    UpdatedAt = f.UpdatedAt
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
        public FinderFormModel UpdateFinderFormStatus(UpdateStatusModel model, Guid updatedBy)
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
            var objectJson = new NotificationToVolunteers { 
            FinderFormId = finderForm.FinderFormId,
            CurrentTime = currentTime,
            InsertedBy = insertedBy,
            path = path
            };

            string json = JsonConvert.SerializeObject(objectJson);

            string FILEPATH = Path.Combine(Directory.GetCurrentDirectory(), "JSON", "NotificationToVolunteers.json");

            System.IO.File.WriteAllText(FILEPATH, json);


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

            if (DateTime.UtcNow >= currentTime.AddMinutes(2))
            {
                if(finderFormService.GetFinderFormById(finderForm.FinderFormId).FinderFormStatus == 1)
                {

                }
            }


            return finderForm;
        }

        public void ReNotification(Guid finderFormId, Guid insertedBy, string path)
        {
            if (uow.GetService<IFinderFormRepository>().GetFinderFormById(finderFormId).FinderFormStatus == 1)
            {
                var centerService = uow.GetService<CenterDomain>();
                var records = centerService.GetListCenter().Select(c => c.CenterId.ToString()).ToList();
                uow.GetService<NotificationTokenDomain>().NotificationForListVolunteerOfCenter(path, records);
                uow.saveChanges();
            }
        }
        #endregion
    }
}
