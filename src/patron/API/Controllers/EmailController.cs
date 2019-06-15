using Commands.Email;
using Core.CQRS;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly CommandBus commandBus;

        public EmailController(CommandBus commandBus)
        {
            this.commandBus = commandBus;

        }

        [HttpPut("{id}")]
        public ActionResult<CommandResult> Put(int id, [FromBody] string email)
        {
            var command = new UpdateEmailCommand(email);
            commandBus.Run(command);

            return Ok(command.Result);

        }

    }
}