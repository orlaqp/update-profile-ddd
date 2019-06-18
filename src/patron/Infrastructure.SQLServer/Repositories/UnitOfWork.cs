using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Core.Domain;
using Core.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider services;

        public UnitOfWork(CommandsDbContext context, IServiceProvider services)
        {
            Context = context;
            this.services = services;
        }

        public DbContext Context { get; }

        public async Task<int> Commit()
        {

            await ProcessDomainEvents();

            return await Context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        private async Task ProcessDomainEvents() {
            var modifiedEntries = Context.ChangeTracker
                .Entries();

            var aggregateRootType = typeof(AggregateRoot);
            foreach (var entity in modifiedEntries)
            {
                var entityType = entity.Entity.GetType();

                if (!entityType.IsSubclassOf(aggregateRootType)) continue;

                var domainEvents = ReflectionHelpers
                    .GetPropertyValue(entity.Entity, entityType, "domainEvents");

                if (domainEvents == null) continue;

                foreach (var domainEvent in (Collection<IDomainEvent>)domainEvents)
                {
                    Console.WriteLine(domainEvent.GetType().FullName);
                    var handler = services.GetService(domainEvent.GetType());

                    if (handler == null) continue;

                    var handlerType = handler.GetType();
                    var handleMethod = handlerType.GetMethod("Handle");

                    Task taskResult = (Task)handleMethod.Invoke(handler, new [] { domainEvent });
                    await taskResult;
                }
            }
        }

        
    }
}