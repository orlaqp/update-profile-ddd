using Core.Domain;
using Domain.Patron.ValueObjects;
using SimpleValidator;

namespace Domain.Patron
{
    public class Patron : AggregateRoot
    {
        protected internal Patron() {}
        private Validator validate = new Validator();

        internal Patron(string firstName, string lastName, EmailAddress emailAddress) 
        {
            validate.IsNotNullOrWhiteSpace(firstName);
            validate.IsNotNull(emailAddress);

            validate.ThrowValidationExceptionIfInvalid();

            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public EmailAddress EmailAddress { get; }
    }
}