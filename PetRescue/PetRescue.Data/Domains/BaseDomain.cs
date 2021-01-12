using PetRescue.Data.Uow;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Domains
{
    public abstract class BaseDomain
    {
        protected readonly IUnitOfWork uow;

        public BaseDomain(IUnitOfWork uow)
        {
            this.uow = uow;
        }
    }
}
