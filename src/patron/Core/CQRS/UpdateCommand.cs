namespace Core.CQRS
{
    public class UpdateCommand<T> : Command
    {
        public UpdateCommand(T id)
        {
            Id = id;
        }

        public T Id { get; }
    }
}