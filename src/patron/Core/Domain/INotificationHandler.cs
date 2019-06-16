namespace Core.Domain
{
    public interface INotificationHandler<T> where T : INotification
    {
         void Handle(T eventDetails);
    }
}