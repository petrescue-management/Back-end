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
        int saveChanges();
        Task<int> saveChangesAsync();
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

        public int saveChanges()
        {
            return this.context.SaveChanges();
        }
        public Task<int> saveChangesAsync()
        {
            return this.context.SaveChangesAsync();
        }
    }
}
