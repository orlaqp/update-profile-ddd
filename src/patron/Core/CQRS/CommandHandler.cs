using System.Threading.Tasks;
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

        public abstract Task Run(T command);

    }
}