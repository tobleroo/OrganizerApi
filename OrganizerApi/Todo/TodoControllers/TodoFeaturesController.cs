using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Todo.TodoServices;
using System.Security.Claims;

namespace OrganizerApi.Todo.TodoControllers
{

    [Route("/todo")]
    [ApiController]
    [Authorize]
    public class TodoFeaturesController : Controller
    {

        private readonly ITodoFeatureService todoService;

        public TodoFeaturesController(ITodoFeatureService todoService)
        {
            this.todoService = todoService;
        }

        [HttpGet("get-suggestions")]
        public async Task<IActionResult> GetTodoSuggestions()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var process = await todoService.CreateTodoSuggestions(name);
            return Ok(process);
        }
    }
}
