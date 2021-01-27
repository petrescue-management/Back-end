using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetTypeRepository : IBaseRepository<PetType, string>
    {
        List<PetTypeModel> GetAllPetTypes();

        PetTypeModel GetPetTypeById(Guid id);
    }

    public partial class PetTypeRepository : BaseRepository<PetType, string>, IPetTypeRepository
    {
        public PetTypeRepository(DbContext context) : base(context)
        {
        }

        public List<PetTypeModel> GetAllPetTypes()
        {
            List<PetTypeModel> types = Get()
               .Select(t => new PetTypeModel
               {
                   PetTypeId = t.PetTypeId,
                   PetTypeName = t.PetTypeName
               }).ToList();

            return types;
        }

        public PetTypeModel GetPetTypeById(Guid id)
        {
            var type = Get()
               .Where(t => t.PetTypeId.Equals(id))
               .Select(t => new PetTypeModel
               {
                   PetTypeId = t.PetTypeId,
                   PetTypeName = t.PetTypeName
               }).FirstOrDefault();

            return type;
        }
    }
}
