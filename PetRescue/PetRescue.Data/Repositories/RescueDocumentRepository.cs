﻿using Microsoft.EntityFrameworkCore;
using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IRescueDocumentRepository : IBaseRepository<RescueDocument, string>
    {
        RescueDocument Create(RescueDocumentCreateModel model, Guid centerId);
        RescueDocument Edit(RescueDocument entity, RescueDocumentUpdateModel model);
    }
    public partial class RescueDocumentRepository : BaseRepository<RescueDocument, string>, IRescueDocumentRepository
    {
        public RescueDocumentRepository(DbContext context) : base(context)
        {
        }
        public RescueDocument Create(RescueDocumentCreateModel model, Guid centerId)
        {
            var result = PrepareCreate(model, centerId);
            return Create(result).Entity;
        }
        private RescueDocument PrepareCreate(RescueDocumentCreateModel model, Guid centerId)
        {
            var RescueDocument = new RescueDocument
            {
                CenterId = centerId,
                FinderFormId = model.FinderFormId,
                RescueDocumentId = Guid.NewGuid(),
                RescueDocumentStatus = RescueDocumentConst.WAITING,
                PickerFormId = model.PickerFormId
            };
            return RescueDocument;
        }

        public RescueDocument Edit(RescueDocument entity, RescueDocumentUpdateModel model)
        {
            entity.RescueDocumentStatus = model.PetDocumentStatus;
            return Update(entity).Entity;
        }
    }
}