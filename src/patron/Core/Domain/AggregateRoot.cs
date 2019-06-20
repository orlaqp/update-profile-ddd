using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.Domain
{
    public abstract class AggregateRoot : Entity
    {
        private List<IDomainEvent> domainEvents;

        protected internal AggregateRoot()
        {
            domainEvents = new List<IDomainEvent>();
        }

        public AggregateRoot(Guid id) : base(id)
        {
        }
        
        protected void QueueDomainEvent(IDomainEvent eventDetails) {
            domainEvents.Add(eventDetails);
        }

        public IEnumerable<IDomainEvent> Events => domainEvents;
    }
}