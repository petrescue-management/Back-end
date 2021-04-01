using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetDocumentRepository : IBaseRepository<PetDocument, string>
    {
    }
    public partial class PetDocumentRepository : BaseRepository<PetDocument, string>, IPetDocumentRepository
    {
        public PetDocumentRepository(DbContext context) : base(context)
        {
        }
    }
}
