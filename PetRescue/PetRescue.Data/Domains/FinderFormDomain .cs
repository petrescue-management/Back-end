using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Extensions;
using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public FinderFormModel CreateFinderForm(CreateFinderFormModel model, Guid insertedBy)
        {
            var finderForm = uow.GetService<IFinderFormRepository>().CreateFinderForm(model, insertedBy);
            uow.saveChanges();
            return finderForm;
        }

        #endregion
    }
}
