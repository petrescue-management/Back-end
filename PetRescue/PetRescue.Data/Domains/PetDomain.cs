using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
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

        public List<PetBreed> GetPetBreedsByTypeId(Guid id)
        {
            var breed_service = uow.GetService<IPetBreedRepository>().Get();
            var breeds = uow.GetService<IPetRepository>().GetPetBreedsByTypeId(id, breed_service);
            return breeds;
        }

        public PetBreed GetPetBreedById(Guid id)
        {
            var breed_service = uow.GetService<IPetBreedRepository>().Get();
            var breed = uow.GetService<IPetRepository>().GetPetBreedById(id, breed_service);
            return breed;
        }
    }
}
