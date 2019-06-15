using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class EmailController : ControllerBase
    {

       [HttpPut("{id}")]
       public ActionResult<CommandResult> Put(int id, [FromBody] string email) {

       }

    }
}