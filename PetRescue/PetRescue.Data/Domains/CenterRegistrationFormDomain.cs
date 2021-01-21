﻿using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class CenterRegistrationFormDomain : BaseDomain
    {
        public CenterRegistrationFormDomain(IUnitOfWork uow) : base(uow)
        {
        }

        public SearchReturnModel SearchCenterRegistrationForm(SearchViewModel model)
        {
            var forms = uow.GetService<ICenterRegistrationFormRepository>().SearchCenterRegistrationForm(model);
            return forms;
        }

        public CenterRegistrationForm GetCenterRegistrationFormById(Guid id)
        {
            var form = uow.GetService<ICenterRegistrationFormRepository>().GetCenterRegistrationFormById(id);
            return form;
        }

        public void UpdateCenterRegistrationForm(UpdateCenterRegistrationFormModel model)
        {
            uow.GetService<ICenterRegistrationFormRepository>().UpdateCenterRegistrationForm(model);
        }

        public string CreateCenterRegistrationForm(CreateCenterRegistrationFormModel model)
        {
            //call UserService
            var user_service = uow.GetService<IUserRepository>();

            //call CenterService
            var center_service = uow.GetService<ICenterRepository>();

            //check Duplicate  phone
            var check_dup_phone = center_service.Get()
                .Where(c => c.Phone.Equals(model.Phone));

            //check Duplicate email
            var check_dup_email = user_service.Get()
               .Where(u => u.UserEmail.Equals(model.Email));

            //check Duplicate address
            var check_dup_address = center_service.Get()
               .Where(c => c.Address.Equals(model.CenterAddress));

            //dup phone & email
            if (check_dup_phone.Any() && check_dup_email.Any())
                return "This phone and email  is already registered !";

            //dup phone & address
            if (check_dup_phone.Any() && check_dup_address.Any())
                return "This phone and address  is already registered !";

            //dup email & address
            if (check_dup_email.Any() && check_dup_address.Any())
                return "This email and address  is already registered !";

            //dup phone
            if (check_dup_phone.Any())
                return "This phone is already registered !";

            //dup email
            if (check_dup_email.Any())
                return "This email is already registered !";

            //dup address
            if (check_dup_address.Any())
                return "This address is already registered !";

            string result = uow.GetService<ICenterRegistrationFormRepository>().CreateCenterRegistrationForm(model);
            return result;
        }
    }
}
