using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class CenterDomain : BaseDomain
    {
        public CenterDomain(IUnitOfWork uow) : base(uow)
        {
        }

        public SearchReturnModel SearchCenter(SearchViewModel model)
        {
            var centers = uow.GetService<ICenterRepository>().SearchCenter(model);        
            return centers;
        }

        public Center GetCenterById(Guid id)
        {
            var center = uow.GetService<ICenterRepository>().GetCenterById(id);
            return center;
        }

        public void DeleteCenter(Guid id)
        {
            uow.GetService<ICenterRepository>().DeleteCenter(id);
        }

        public void CreateCenter(CreateCenterModel model)
        {
            uow.GetService<ICenterRepository>().CreateCenter(model);
        }
    }
}
