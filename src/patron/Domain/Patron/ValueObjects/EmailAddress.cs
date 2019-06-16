using SimpleValidator;

namespace Domain.Patron.ValueObjects
{
    public class EmailAddress
    {
        Validator validate = new Validator();
        public EmailAddress(string email)
        {
            validate.IsEmail(email);
            validate.ThrowValidationExceptionIfInvalid();

            Email = email;
        }

        public string Email { get; }
    }
}