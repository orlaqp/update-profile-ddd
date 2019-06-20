namespace Core.Domain
{
    public interface IDomainEvent
    {
        bool Auditable { get; }
    }
}