using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Domain
{
    public interface IDomainEventBus
    {
         Task Handle(IEnumerable<IDomainEvent> domainEvents);
         Task Handle(IDomainEvent domainEvent);
    }
}