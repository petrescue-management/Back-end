using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PetRescue.Data.Repositories
{
    public partial interface IBaseRepository
    {

    }
    public partial interface IBaseRepository<E, K> where E : class
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        EntityEntry<E> Create(E entity);
        EntityEntry<E> Update(E entity);
        EntityEntry<E> Remove(E entity);
        DbSet<E> Get();

    }
    public partial class BaseRepository<E, K> : IBaseRepository<E, K> where E : class
    {
        protected readonly DbContext context;
        protected readonly DbSet<E> dbSet;
        public BaseRepository(DbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<E>();
        }
        public EntityEntry<E> Create(E entity)
        {
            return dbSet.Add(entity);
        }

        public DbSet<E> Get()
        {
            return this.dbSet;
        }

        public EntityEntry<E> Remove(E entity)
        {
            return this.dbSet.Remove(entity);
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }

        public EntityEntry<E> Update(E entity)
        {
            return dbSet.Update(entity);
        }
    }
}
