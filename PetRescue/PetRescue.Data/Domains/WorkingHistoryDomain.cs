using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class WorkingHistoryDomain : BaseDomain
    {
        public WorkingHistoryDomain(IUnitOfWork uow) : base(uow)
        {
        }
        public object GetListWorkingHistoryById(Guid userId)
        {
            var workingHistoryRepo = uow.GetService<IWorkingHistoryRepository>();
            var workingHistorys = workingHistoryRepo.Get().Where(s => s.UserId.Equals(userId));
            if(workingHistorys != null)
            {

            }
            return null;
        }
    }
}
