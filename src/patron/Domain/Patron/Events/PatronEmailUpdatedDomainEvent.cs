using Core.Domain;
using Domain.Patron.Events;
using Domain.Patron.ValueObjects;

namespace Domain.Patron.Events
{
    public class PatronEmailUpdatedDomainEvent : IDomainEvent
    {
        public PatronEmailUpdatedDomainEvent(EmailAddress newEmail) 
        {
            NewEmail = newEmail;
        }

        public EmailAddress NewEmail { get; }
    }
}