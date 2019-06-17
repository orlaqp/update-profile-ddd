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

        protected Repository(DbContext context)
        {
            this.context = context;
        }

        protected T ById(Guid id)
        {
            throw new NotImplementedException();
        }

        protected void Add(T aggregateRoot)
        {
            throw new NotImplementedException();
        }

        protected void Delete(T aggregateRoot)
        {
            throw new NotImplementedException();
        }

        protected IQueryable<T> Search(Func<T, bool> cond)
        {
            throw new NotImplementedException();
        }

        protected void Update(T aggregateRoot)
        {
            throw new NotImplementedException();
        }
    }
}