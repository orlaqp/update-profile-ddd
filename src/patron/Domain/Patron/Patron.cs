using Core.Domain;
using Domain.Patron.Factories;
using Domain.Patron.ValueObjects;
using SimpleValidator;

namespace Domain.Patron
{
    public class Patron : AggregateRoot
    {
        protected internal Patron() {}
        private Validator validate = new Validator();

        internal Patron(INewPatronData data) 
        {
            validate.IsNotNullOrWhiteSpace(data.FirstName);
            validate.IsNotNull(data.Email);

            validate.ThrowValidationExceptionIfInvalid();

            FirstName = data.FirstName;
            LastName = data.LastName;
            EmailAddress = data.Email;
        }

        public string FirstName { get; private set;}
        public string LastName { get; private set; }
        public EmailAddress EmailAddress { get; private set; }
    }
}