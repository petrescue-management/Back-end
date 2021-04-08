using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetDocumentRepository : IBaseRepository<PetDocument, string>
    {
        PetDocument Create();
        PetDocument Edit(PetDocument entity, PetDocumentUpdateModel model);
    }
    public partial class PetDocumentRepository : BaseRepository<PetDocument, string>, IPetDocumentRepository
    {
        public PetDocumentRepository(DbContext context) : base(context)
        {
        }

        public PetDocument Create()
        {
            throw new NotImplementedException();
        }

        public PetDocument Edit(PetDocument entity, PetDocumentUpdateModel model)
        {
            entity.PetDocumentStatus = model.PetDocumentStatus;
            return Update(entity).Entity;
        }
    }
}
