using Serilog;

namespace Core.CQRS
{
    public abstract class CommandHandler<T> where T : Command
    {
        protected readonly ILogger logger;

        public CommandHandler(ILogger logger)
        {
            this.logger = logger;

        }

    }
}