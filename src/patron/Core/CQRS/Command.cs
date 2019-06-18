namespace Core.CQRS
{
    public class Command
    {
        public Command()
        {
            Result = new CommandResult();
        }
        public CommandResult Result { get; set; }
    }
}