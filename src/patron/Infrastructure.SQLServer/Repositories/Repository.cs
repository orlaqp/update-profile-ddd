using System;
using System.Linq;
using Core.Domain;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer.Repositories
{
    public abstract class Repository<T> where T : AggregateRoot
    {
        private readonly DbContext context;
        private readonly DbSet<T> set;

        protected Repository(DbContext context)
        {
            this.context = context;
            this.set = context.Set<T>();
        }

        protected T ById(Guid id)
        {
            return set.FirstOrDefault(p => p.Id == id);
        }

        protected void Insert(T aggregateRoot)
        {
            set.Add(aggregateRoot);
        }

        protected void Delete(T aggregateRoot)
        {
            set.Remove(aggregateRoot);
        }

        protected IQueryable<T> Search(Func<T, bool> cond)
        {
            return set.Where(cond).AsQueryable();
        }

        protected void Update(T aggregateRoot)
        {
            set.Update(aggregateRoot);
        }
    }
}