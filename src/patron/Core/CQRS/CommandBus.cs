using System;
using System.Threading.Tasks;
using Core.Exceptions;
using Core.Helpers;
using Serilog;
using SimpleValidator.Exceptions;

namespace Core.CQRS
{
    public class CommandBus : ICommandBus
    {
        private readonly IServiceProvider services;
        private readonly ILogger logger;
        private Type commandHandler = typeof(CommandHandler<>);

        public CommandBus(IServiceProvider services, ILogger logger)
        {
            this.services = services;
            this.logger = logger;
        }

        public async Task Run(Command command)
        {
            try
            {
                var commandType = command.GetType();
                var handler = services.GetService(commandType);

                if (handler == null) {
                    logger.Error("Handler not found for: " + commandType.FullName);
                }

                await ReflectionHelpers.InvokeAsyncMethod(handler, "Run", new [] { command });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null) {
                    if (ex.InnerException.GetType() == typeof(ValidationException)) {
                        command.Result.ValidationErrors(ex.InnerException as ValidationException);
                    } else if (ex.InnerException.GetType() == typeof(BusinessException)) {
                        command.Result.Error(ex as BusinessException);
                    }
                } else {
                    logger.Error(ex, $"Error executing command {command.GetType().FullName}");
                    command.Result.UnexpectedError();
                }

            }
            
        }
    }
}