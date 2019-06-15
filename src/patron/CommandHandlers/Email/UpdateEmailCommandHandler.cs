using System;
using Commands.Email;
using Core.CQRS;
using Serilog;

namespace CommandHandlers.Email
{
    public class UpdateEmailCommandHandler : ICommandHandler<UpdateEmailCommand>
    {
        private readonly ILogger logger;
        public UpdateEmailCommandHandler(ILogger logger)
        {
            this.logger = logger;
        }
        public void Run(UpdateEmailCommand command)
        {
            logger.Debug("****** Executing UpdateEmailCommandHandler ******");
        }
    }
}