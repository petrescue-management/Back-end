using Microsoft.EntityFrameworkCore;
using PetRescue.Data.Models;
using PetRescue.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PetRescue.Data.Repositories
{
    public partial interface IWorkingHistoryRepository : IBaseRepository<WorkingHistory, string>
    {
        WorkingHistory Create(WorkingHistoryCreateModel model);
        WorkingHistory Edit(WorkingHistoryUpdateModel model);
    }
    public class WorkingHistoryRepository : BaseRepository<WorkingHistory, string>, IWorkingHistoryRepository
    {
        public WorkingHistoryRepository(DbContext context) : base(context)
        {
        }

        public WorkingHistory Create(WorkingHistoryCreateModel model)
        {
            var workingHistory = PrepareCreate(model);
            return Create(workingHistory).Entity;
        }
        private WorkingHistory PrepareCreate(WorkingHistoryCreateModel model)
        {
            return new WorkingHistory
            {
                CenterId = model.CenterId,
                DateStarted = DateTime.UtcNow,
                Description = model.Description,
                IsActive = true,
                WorkingHistoryId = Guid.NewGuid(),
                RoleName = model.RoleName,
                UserId = model.UserId,
            };
        }

        public WorkingHistory Edit(WorkingHistoryUpdateModel model)
        {
            var workingHistory = PrepareUpdate(model);
            return Update(workingHistory).Entity;
        }
        private WorkingHistory PrepareUpdate(WorkingHistoryUpdateModel model)
        {
            var workingHistory = Get().FirstOrDefault(s => s.WorkingHistoryId.Equals(model.WorkingHistoryId));
            workingHistory.IsActive = model.IsActive;
            workingHistory.Description = model.Description;
            return workingHistory;
        }
    }
}
