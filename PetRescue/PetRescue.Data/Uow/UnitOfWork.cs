using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PetRescue.Data.Uow
{
    public partial interface IUnitOfWork
    {
        T GetService<T>();
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }

    public partial class UnitOfWork : IUnitOfWork
    {
        protected readonly IServiceProvider scope;
        protected readonly DbContext context;

        public UnitOfWork(IServiceProvider scope, DbContext context)
        {
            this.scope = scope;
            this.context = context;
        }
        public T GetService<T>()
        {
            return scope.GetService<T>();
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return this.context.SaveChangesAsync();
        }
    }
}
