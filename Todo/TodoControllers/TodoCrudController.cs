using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Todo.models;
using OrganizerApi.Todo.service;
using System.Security.Claims;

namespace OrganizerApi.Todo.TodoControllers
{
    [ApiController]
    [Route("todo")]
    [Authorize]
    public class TodoCrudController : Controller
    {

        private readonly ITodoService _todoService;

        public TodoCrudController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("all-tasks")]
        public async Task<IActionResult> GetTodoData()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var todoData = await _todoService.GetTodoData(name);

            return Ok(todoData);
        }

        [HttpPost("update-tasks")]
        public async Task<IActionResult> UpdateTodoData([FromBody] TodoDocument todoDb)
        {
            var res = await _todoService.UpdateTodoData(todoDb);
            if (!res)
            {
                return BadRequest();
            }else
            {
                return Ok();
            }
        }
    }
}
