using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.Domain
{
    public abstract class AggregateRoot : Entity
    {

        protected internal AggregateRoot()
        {   
        }

        public AggregateRoot(Guid id) : base(id)
        {
        }
        protected Collection<IDomainEvent> domainEvents;
        
        protected void AddDomainEvent(IDomainEvent eventDetails) {
            domainEvents = domainEvents ?? new Collection<IDomainEvent>();
            domainEvents.Add(eventDetails);
        }
    }
}