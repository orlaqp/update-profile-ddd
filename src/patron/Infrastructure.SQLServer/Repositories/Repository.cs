using System;
using System.Linq;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer.Repositories
{
    public abstract class Repository<T> where T : AggregateRoot
    {
        private readonly DbSet<T> set;
        private readonly IUnitOfWork unitOfWork;

        protected Repository(IUnitOfWork unitOfWork)
        {
            this.set = unitOfWork.Context.Set<T>();
            this.unitOfWork = unitOfWork;
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
            T existing = set.Find(aggregateRoot);

            if (existing == null) {
                return;
            }

            set.Remove(existing);
        }

        protected IQueryable<T> Search(Func<T, bool> cond)
        {
            return set.Where(cond).AsQueryable();
        }

        protected void Update(T aggregateRoot)
        {
            unitOfWork.Context.Entry(aggregateRoot).State = EntityState.Modified;
            set.Attach(aggregateRoot);
        }
    }
}