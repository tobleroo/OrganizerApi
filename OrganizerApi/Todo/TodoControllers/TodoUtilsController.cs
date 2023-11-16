using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OrganizerApi.Todo.TodoControllers
{
    [Route("todo-services")]
    [ApiController]
    public class TodoUtilsController : ControllerBase
    {

        [HttpGet("monthly-plan")]
        public IActionResult GetMonthlyTodoPlan()
        {
            return Ok();
        }
    }
}
