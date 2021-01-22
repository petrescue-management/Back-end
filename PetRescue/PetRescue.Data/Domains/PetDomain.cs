using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class PetDomain : BaseDomain
    {
        public PetDomain(IUnitOfWork uow) : base(uow)
        {
        }

        public List<PetBreedModel> GetPetBreedsByTypeId(Guid id)
        {
            var breeds = uow.GetService<IPetBreedRepository>().GetPetBreedsByTypeId(id);
            return breeds;
        }

        public PetBreedModel GetPetBreedById(Guid id)
        {
            var breed = uow.GetService<IPetBreedRepository>().GetPetBreedById(id);
            return breed;
        }

        public List<PetFurColorModel> GetAllPetFurColors()
        {
            var colors = uow.GetService<IPetFurColorRepository>().GetAllPetFurColors();
            return colors;
        }

        public PetFurColorModel GetPetFurColorById(Guid id)
        {
            var color = uow.GetService<IPetFurColorRepository>().GetPetFurColorById(id);
            return color;
        }

        public List<PetTypeModel> GetAllPetTypes()
        {
            var types = uow.GetService<IPetTypeRepository>().GetAllPetTypes();
            return types;
        }

        public PetTypeModel GetPetTypeById(Guid id)
        {
            var type = uow.GetService<IPetTypeRepository>().GetPetTypeById(id);
            return type;
        }
    }
}
