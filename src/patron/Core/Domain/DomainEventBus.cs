using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Helpers;

namespace Core.Domain
{
    public class DomainEventBus : IDomainEventBus
    {
        private readonly IServiceProvider services;

        public DomainEventBus(IServiceProvider services)
        {
            this.services = services;
        }

        public async Task Handle(IEnumerable<IDomainEvent> domainEvents) {
            foreach (var domainEvent in domainEvents)
            {
                await this.Handle(domainEvent);
            }
        }

        public async Task Handle(IDomainEvent domainEvent) {
            var handler = services.GetService(domainEvent.GetType());

            if (handler == null) return;

            await ReflectionHelpers.InvokeAsyncMethod(handler, "Handle", new [] { domainEvent });
        }
        
    }
}