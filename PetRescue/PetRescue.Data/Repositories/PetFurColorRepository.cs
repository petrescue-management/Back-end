using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IPetFurColorRepository : IBaseRepository<PetFurColor, string>
    {
        List<PetFurColorModel> GetAllPetFurColors();

        PetFurColorModel GetPetFurColorById(Guid id);

        PetFurColor Create(PetFurColorCreateModel model);
        PetFurColor PrepareCreate(PetFurColorCreateModel model);
        PetFurColor Edit(PetFurColorUpdateModel model, PetFurColor entity);

        PetFurColor Remove(Guid petFurColorId);
    }

    public partial class PetFurColorRepository : BaseRepository<PetFurColor, string>, IPetFurColorRepository
    {
        public PetFurColorRepository(DbContext context) : base(context)
        {
        }

        public PetFurColor Create(PetFurColorCreateModel model)
        {
            var newPetFurColor = PrepareCreate(model);
            return Create(newPetFurColor).Entity;
        }

        public PetFurColor Edit(PetFurColorUpdateModel model, PetFurColor entity)
        {
            entity.PetFurColorName = model.PetFurColorName;
            return  Update(entity).Entity;
        }

        public List<PetFurColorModel> GetAllPetFurColors()
        {
            List<PetFurColorModel> colors = Get()
               .Select(c => new PetFurColorModel
               {
                   PetFurColorId = c.PetFurColorId,
                   PetFurColorName = c.PetFurColorName
               }).ToList();

            return colors;
        }

        public PetFurColorModel GetPetFurColorById(Guid id)
        {
            var color = Get()
               .Where(c => c.PetFurColorId.Equals(id))
               .Select(c => new PetFurColorModel
               {
                   PetFurColorId = c.PetFurColorId,
                   PetFurColorName = c.PetFurColorName
               }).FirstOrDefault();

            return color;
        }

        public PetFurColor PrepareCreate(PetFurColorCreateModel model)
        {
            var newPetFurColor = new PetFurColor
            {
                PetFurColorId = Guid.NewGuid(),
                PetFurColorName = model.PetFurColorName
            };
            return newPetFurColor;
        }

        public PetFurColor Remove(Guid petFurColorId)
        {
            throw new NotImplementedException();
        }
    }
}
