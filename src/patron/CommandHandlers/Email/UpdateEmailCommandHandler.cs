using Commands.Email;
using Core.CQRS;

namespace CommandHandlers.Email
{
    public class UpdateEmailCommandHandler : ICommandHandler<UpdateEmailCommand>
    {
        public void Run(UpdateEmailCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}