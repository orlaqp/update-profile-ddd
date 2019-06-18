using System;
using System.Threading.Tasks;
using Core.Domain;
using Serilog;

namespace Domain.Patron.Events
{
    public class PatronEmailUpdatedDomainEventHandler : DomainEventHandler<PatronEmailUpdatedDomainEvent>
    {
        public PatronEmailUpdatedDomainEventHandler(ILogger logger) : base(logger)
        {
        }

        public override async Task Handle(PatronEmailUpdatedDomainEvent notificationDetails)
        {
            Console.WriteLine("Handling PatronEmailUpdatedDomainEvent");
        }
    }
}