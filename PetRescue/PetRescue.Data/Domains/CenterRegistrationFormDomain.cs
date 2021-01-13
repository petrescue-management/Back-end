﻿using PetRescue.Data.Models;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
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
    }
}
