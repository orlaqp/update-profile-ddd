using Core.CQRS;

namespace Commands.Email
{
    public class UpdateEmailCommand : Command
    {
        public UpdateEmailCommand(string email)
        {
            Email = email;
        }

        public string Email { get; }
    }
}