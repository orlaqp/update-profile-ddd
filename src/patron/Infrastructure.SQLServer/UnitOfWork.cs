using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider services;
        private readonly IDomainEventBus domainEventBus;

        public UnitOfWork(
            CommandsDbContext context,
            IServiceProvider services,
            IDomainEventBus domainEventBus)
        {
            Context = context;
            this.services = services;
            this.domainEventBus = domainEventBus;
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
            var aggregateRootType = typeof(AggregateRoot);
            var aggRootEntries = Context.ChangeTracker
                .Entries()
                .Select(e => e.Entity)
                .Where(e => e.GetType().IsSubclassOf(aggregateRootType));

            foreach (var aggRoot in aggRootEntries)
            {
                await domainEventBus.Handle((aggRoot as AggregateRoot).Events);
            }
        }

        
    }
}