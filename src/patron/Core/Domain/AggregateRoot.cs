using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.Domain
{
    public abstract class AggregateRoot
    {
        protected Collection<INotification> events;
        
        protected void AddEvent(INotification eventDetails) {
            events = events ?? new Collection<INotification>();
            events.Add(eventDetails);
        }
    }
}