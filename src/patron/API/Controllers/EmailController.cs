using System;
using System.Threading.Tasks;
using Commands.Email;
using Core.CQRS;
using Infrastructure.SQLServer.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("email")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly CommandBus commandBus;

        public EmailController(IUnitOfWork unitOfWork, CommandBus commandBus)
        {
            this.unitOfWork = unitOfWork;
            this.commandBus = commandBus;
        }

        /// <summary>
        /// Updates patron's email address
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<CommandResult>> Put(Guid id, [FromBody] string email)
        {
            var command = new UpdateEmailCommand(id, email);
            await commandBus.Run(command);
            await unitOfWork.Commit();

            return Ok(command.Result);
        }

    }
}