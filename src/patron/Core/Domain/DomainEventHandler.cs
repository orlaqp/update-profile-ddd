using System.Threading.Tasks;
using Serilog;

namespace Core.Domain
{
    public abstract class DomainEventHandler<T> where T : IDomainEvent
    {
        public DomainEventHandler(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }

        public abstract Task Handle(T notificationDetails);
    }
}