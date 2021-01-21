using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class CenterDomain : BaseDomain
    {
        public CenterDomain(IUnitOfWork uow) : base(uow)
        {
        }

        public SearchReturnModel SearchCenter(SearchModel model)
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

        public string UpdateCenter(UpdateCenterModel model)
        {
            //call CenterService
            var center_service = uow.GetService<ICenterRepository>();

            //check Duplicate  phone
            var check_dup_phone = center_service.Get()
                .Where(c => c.Phone.Equals(model.Phone));

            //check Duplicate address
            var check_dup_address = center_service.Get()
               .Where(c => c.Address.Equals(model.Address));

            //dup phone & address
            if (check_dup_phone.Any() && check_dup_address.Any())
                return "This phone and address  is already registered !";

            //dup phone
            if (check_dup_phone.Any())
                return "This phone is already registered !";

            //dup address
            if (check_dup_address.Any())
                return "This address is already registered !";

            var result = center_service.UpdateCenter(model);
            return result;
        }
    }
}
