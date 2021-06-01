using PetRescue.Data.Uow;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Domains
{
    public abstract class BaseDomain
    {
        protected readonly IUnitOfWork _uow;

        public BaseDomain(IUnitOfWork uow)
        {
            this._uow = uow;
        }
    }
}
