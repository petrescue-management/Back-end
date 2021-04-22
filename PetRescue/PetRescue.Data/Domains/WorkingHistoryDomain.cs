using PetRescue.Data.Uow;
using System;
using System.Collections.Generic;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class WorkingHistoryDomain : BaseDomain
    {
        public WorkingHistoryDomain(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
