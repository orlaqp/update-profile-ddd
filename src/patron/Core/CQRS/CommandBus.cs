using System;
using Serilog;

namespace Core.CQRS
{
    public class CommandBus
    {
        private readonly IServiceProvider services;
        private readonly ILogger logger;
        private Type commandHandler = typeof(CommandHandler<>);

        public CommandBus(IServiceProvider services, ILogger logger)
        {
            this.services = services;
            this.logger = logger;
        }

        public void Run(Command command)
        {
            try
            {
                var commandType = command.GetType();
                var handler = services.GetService(commandType);

                if (handler == null) {
                    logger.Error("Handler not found for: " + commandType.FullName);
                }

                var runMethod = handler.GetType().GetMethod("Run");
                runMethod.Invoke(handler, new object[] { command });    
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Error executing command {command.GetType().FullName}");
                throw;
            }
            
        }
    }
}