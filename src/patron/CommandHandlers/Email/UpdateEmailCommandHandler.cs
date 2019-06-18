using System;
using Commands.Email;
using Core.CQRS;
using Domain.Patron.ValueObjects;
using Infrastructure.Repositories;
using Serilog;

namespace CommandHandlers.Email
{
    public class UpdateEmailCommandHandler : CommandHandler<UpdateEmailCommand>
    {
        private readonly IPatronRepository patrons;

        public UpdateEmailCommandHandler(
            ILogger logger,
            IPatronRepository patrons
        ) : base(logger)
        {
            this.patrons = patrons;
        }

        public void Run(UpdateEmailCommand command)
        {
            logger.Debug("****** Executing UpdateEmailCommandHandler ******");
            var email = new EmailAddress(command.Email);

            var patron = patrons.GetById(command.Id);

            if (patron == null) {
                throw new InvalidOperationException($"Patron {command.Id} not found");
            }

            patron.UpdateEmail(email);
        }
    }
}