
using System;

namespace Core.CQRS
{
    public class CommandBus
    {
        private readonly IServiceProvider services;
        private Type commandHandler = typeof(ICommandHandler<>);

        public CommandBus(IServiceProvider services)
        {
            this.services = services;
        }

        public void Run(Command command)
        {
            var handlerRegistration = commandHandler.MakeGenericType(command.GetType());
            var handler = services.GetService(handlerRegistration);

            if (handler == null) {
                Console.WriteLine("Handler not found for: " + handlerRegistration.FullName);
            }

            var runMethod = handler.GetType().GetMethod("Run");
            runMethod.Invoke(handler, new object[] { command });
        }
    }
}