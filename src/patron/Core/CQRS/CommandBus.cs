using System;
using System.Threading.Tasks;
using Core.Exceptions;
using Serilog;
using SimpleValidator.Exceptions;

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

        public async Task Run(Command command)
        {
            try
            {
                var commandType = command.GetType();
                var handler = services.GetService(commandType);

                if (handler == null) {
                    logger.Error("Handler not found for: " + commandType.FullName);
                }

                var runMethod = handler.GetType().GetMethod("Run");
                Task result = (Task)runMethod.Invoke(handler, new object[] { command });    
                await result;
            }
            catch (BusinessException ex) {
                command.Result.Error(ex.Code, ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(ValidationException)) {
                    command.Result.ValidationErrors(ex.InnerException as ValidationException);
                } else {
                    logger.Error(ex, $"Error executing command {command.GetType().FullName}");
                    command.Result.UnexpectedError();
                }

            }
            
        }

        public void Commit() {

        }
    }
}