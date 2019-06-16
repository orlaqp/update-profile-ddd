using System;
using Core.Domain;

namespace Infrastructure.Repositories
{
    public interface IRepository<T> where T : AggregateRoot
    {
         void Add(T aggregateRoot);
         void Delete(T aggregateRoot);
         void Update(T aggregateRoot);
         void Search(Func<T, bool> cond);
    }
}