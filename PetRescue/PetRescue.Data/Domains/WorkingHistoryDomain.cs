using PetRescue.Data.ConstantHelper;
using PetRescue.Data.Repositories;
using PetRescue.Data.Uow;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Domains
{
    public class WorkingHistoryDomain : BaseDomain
    {
        private readonly IWorkingHistoryRepository _workingHistoryRepo;
        public WorkingHistoryDomain(IUnitOfWork uow, IWorkingHistoryRepository workingHistoryRepo) : base(uow)
        {
            this._workingHistoryRepo = workingHistoryRepo;
        }
        public object GetListWorkingHistoryById(Guid userId)
        {
            var workingHistorys = _workingHistoryRepo.Get().Where(s => s.UserId.Equals(userId));
            var result = new List<WorkingHistoryViewModel>();
            if(workingHistorys != null)
            {
                foreach(var workingHistory in workingHistorys)
                {
                    result.Add(new WorkingHistoryViewModel
                    {
                        CenterAddress = workingHistory.Center.Address,
                        CenterName = workingHistory.Center.CenterName,
                        DateEnded = workingHistory.DateEnded?.AddHours(ConstHelper.UTC_VIETNAM),
                        DateStarted = workingHistory.DateStarted.AddHours(ConstHelper.UTC_VIETNAM),
                        Description = workingHistory.Description,
                        IsActice = workingHistory.IsActive,
                        RoleName = workingHistory.RoleName,
                    });
                }
            }
            return result;
        }
    }
}
