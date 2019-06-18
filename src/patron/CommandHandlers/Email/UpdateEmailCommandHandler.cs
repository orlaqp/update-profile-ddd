using System;
using System.Threading.Tasks;
using Commands.Email;
using Core.CQRS;
using Core.Exceptions;
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

        public async override Task Run(UpdateEmailCommand command)
        {
            logger.Debug("****** Executing UpdateEmailCommandHandler ******");
            var email = new EmailAddress(command.Email);

            var patron = await patrons.GetById(command.Id);

            if (patron == null) {
                throw new BusinessException("PatronNotFound", $"Patron {command.Id} not found");
            }

            patron.UpdateEmail(email);
        }

        // public override Task Run(UpdateEmailCommand command)
        // {
        //     throw new NotImplementedException();
        // }
    }
}