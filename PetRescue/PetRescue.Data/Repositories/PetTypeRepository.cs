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
        PetType Create(PetTypeCreateModel model);
        PetType PrepareCreate(PetTypeCreateModel model);
        PetType Edit(PetType entity, PetTypeUpdateModel model);
    }

    public partial class PetTypeRepository : BaseRepository<PetType, string>, IPetTypeRepository
    {
        public PetTypeRepository(DbContext context) : base(context)
        {
        }

        public PetType Create(PetTypeCreateModel model)
        {
            var newPetType = PrepareCreate(model);
            Create(newPetType);
            return newPetType;
        }

        public PetType Edit(PetType entity, PetTypeUpdateModel model)
        {
           if(model.PetTypeName != null)
            {
                entity.PetTypeName = model.PetTypeName;
            }
            return entity;
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

        public PetType PrepareCreate(PetTypeCreateModel model)
        {
            var newPetType = new PetType
            {
                PetTypeId = Guid.NewGuid(),
                PetTypeName = model.PetTypeName
            };
            return newPetType;
        }
    }
}
