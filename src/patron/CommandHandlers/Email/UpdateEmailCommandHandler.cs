using System;
using Commands.Email;
using Core.CQRS;
using Serilog;

namespace CommandHandlers.Email
{
    public class UpdateEmailCommandHandler : CommandHandler<UpdateEmailCommand>
    {
        public UpdateEmailCommandHandler(ILogger logger) : base(logger)
        {
        }

        public void Run(UpdateEmailCommand command)
        {
            logger.Debug("****** Executing UpdateEmailCommandHandler ******");
        }
    }
}