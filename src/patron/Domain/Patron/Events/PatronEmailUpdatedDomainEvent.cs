using System;
using Core.Domain;
using Domain.Patron.ValueObjects;

namespace Domain.Patron.Events
{
    public class PatronEmailUpdatedDomainEvent : IDomainEvent
    {
        public PatronEmailUpdatedDomainEvent(Guid patronId, string newAddress) 
        {
            PatronId = patronId;
            NewAddress = newAddress;
        }

        public Guid PatronId { get; }
        public string NewAddress { get; }
        public bool Auditable => true;
    }
}