using System;
using Commands.Email;
using Core.CQRS;

namespace CommandHandlers.Email
{

    public interface TestInterface {
        
    }

    public class UpdateEmailCommandHandler : ICommandHandler<UpdateEmailCommand>, TestInterface
    {
        public void Run(UpdateEmailCommand command)
        {
            Console.WriteLine("Executing UpdateEmailCommandHandler");
        }
    }
}