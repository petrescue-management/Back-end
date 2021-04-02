using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IFinderFormRepository : IBaseRepository<FinderForm, string>
    {

        FinderFormModel GetFinderFormById(Guid id);

        FinderFormModel UpdateFinderFormStatus(UpdateStatusModel model, Guid updateBy);

        FinderFormModel CreateFinderForm(CreateFinderFormModel model, Guid insertedBy);
    }
    public partial class FinderFormRepository : BaseRepository<FinderForm, string>, IFinderFormRepository
    {
        public FinderFormRepository(DbContext context) : base(context)
        {
        }

        #region CREATE
        private FinderForm PrepareCreate(CreateFinderFormModel model, Guid insertedBy)
        {

            var finderForm = new FinderForm
            {
                FinderFormId = Guid.NewGuid(),
                Lat = model.Lat,
                Lng = model.Lng,
                FinderFormImgUrl = model.FinderFormImgUrl,
                PetAttribute = model.PetAttribute,             
                FinderDescription = model.FinderDescription,
                FinderFormStatus = 1,
                Phone = model.Phone,
                InsertedBy = insertedBy,
                InsertedAt = DateTime.UtcNow,
            };
            return finderForm;
        }


        public FinderFormModel CreateFinderForm(CreateFinderFormModel model, Guid insertedBy)
        {
            var finderForm = PrepareCreate(model, insertedBy);

            Create(finderForm);

            var result = new FinderFormModel
            {
                FinderFormId = finderForm.FinderFormId,
                Lat = finderForm.Lat,
                Lng = finderForm.Lng,
                FinderFormImgUrl = finderForm.FinderFormImgUrl,
                PetAttribute = finderForm.PetAttribute,
                FinderDescription = finderForm.FinderDescription,
                FinderFormStatus = finderForm.FinderFormStatus,
                Phone = finderForm.Phone,
                InsertedBy = finderForm.InsertedBy,
                InsertedAt = finderForm.InsertedAt,
            };
            return result;

        }
        #endregion

        #region GET BY ID
        public FinderFormModel GetFinderFormById(Guid id)
        {
            var result = Get()
                .Where(f => f.FinderFormId.Equals(id))
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
                }).FirstOrDefault();
            return result;
        }
        #endregion

        #region UPDATE STATUS
        private FinderForm PrepareUpdate(UpdateStatusModel model, Guid updatedBy)
        {
            var finderForm = Get()
                    .Where(f => f.FinderFormId.Equals(model.Id))
                    .Select(f => new FinderForm
                    {
                        FinderFormId = model.Id,
                        Lat = f.Lat,
                        Lng = f.Lng,
                        FinderFormImgUrl = f.FinderFormImgUrl,
                        PetAttribute = f.PetAttribute,
                        FinderDescription = f.FinderDescription,
                        FinderFormStatus = model.Status,
                        Phone = f.Phone,
                        InsertedBy = f.InsertedBy,
                        InsertedAt = f.InsertedAt,
                    }).FirstOrDefault();

            return finderForm;
        }
       public FinderFormModel UpdateFinderFormStatus(UpdateStatusModel model, Guid updatedBy)
       {
            var finderForm = PrepareUpdate(model, updatedBy);

            Update(finderForm);

            var result = Get()
                    .Where(f => f.FinderFormId.Equals(model.Id))
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
                    }).FirstOrDefault();
            
            return result;
       }
        #endregion

    }
}
