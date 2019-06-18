using SimpleValidator;

namespace Domain.Patron.ValueObjects
{
    public class EmailAddress
    {

        protected internal EmailAddress()
        {   
        }

        Validator validate = new Validator();
        public EmailAddress(string email)
        {
            validate.IsEmail(nameof(Email), email);
            validate.ThrowValidationExceptionIfInvalid();

            Email = email;
        }

        public string Email { get; private set; }
    }
}