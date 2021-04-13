using Microsoft.EntityFrameworkCore;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetDocumentRepository : IBaseRepository<PetDocument, string>
    {
        PetDocument Create(PetDocumentCreateModel model, Guid centerId);
        PetDocument Edit(PetDocument entity, PetDocumentUpdateModel model);
    }
    public partial class PetDocumentRepository : BaseRepository<PetDocument, string>, IPetDocumentRepository
    {
        public PetDocumentRepository(DbContext context) : base(context)
        {
        }
        public PetDocument Create(PetDocumentCreateModel model, Guid centerId)
        {
            var result = PrepareCreate(model, centerId);
            return Create(result).Entity;
        }
        private PetDocument PrepareCreate(PetDocumentCreateModel model, Guid centerId)
        {
            var petDocument = new PetDocument
            {
                CenterId = centerId,
                FinderFormId = model.FinderFormId,
                PetDocumentId = Guid.NewGuid(),
                PetDocumentStatus = PetDocumentConst.WAITING,
                PickerFormId = model.PickerFormId
            };
            return petDocument;
        }

        public PetDocument Edit(PetDocument entity, PetDocumentUpdateModel model)
        {
            entity.PetDocumentStatus = model.PetDocumentStatus;
            return Update(entity).Entity;
        }
    }
}
