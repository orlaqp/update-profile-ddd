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

        public abstract void Handle(T notificationDetails);
    }
}