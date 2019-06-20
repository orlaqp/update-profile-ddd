using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Audit;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SQLServer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IServiceProvider services;
        private readonly IDomainEventBus domainEventBus;
        private readonly IAuditor auditor;

        public UnitOfWork(
            CommandsDbContext context,
            IServiceProvider services,
            IDomainEventBus domainEventBus,
            IAuditor auditor)
        {
            Context = context;
            this.services = services;
            this.domainEventBus = domainEventBus;
            this.auditor = auditor;
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
                foreach (var domainEvent in (aggRoot as AggregateRoot).Events) {
                    await domainEventBus.Handle(domainEvent);

                    if (domainEvent.Auditable) {
                        await auditor.Audit(domainEvent);
                    }
                }

            }
        }

        
    }
}