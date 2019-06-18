using System;
using Core.CQRS;

namespace Commands.Email
{
    public class UpdateEmailCommand : UpdateCommand<Guid>
    {
        public UpdateEmailCommand(Guid id, string email) : base(id)
        {
            Email = email;
        }

        public string Email { get; }
    }
}