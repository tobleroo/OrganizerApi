using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizerApi.Todo.TodoServices;
using OrganizerBlazor.Todo.Models;
using System.Security.Claims;

namespace OrganizerApi.Todo.TodoControllers
{

    [Route("/todo")]
    [ApiController]
    [Authorize]
    public class TodoCrudController : Controller
    {

        private readonly ITodoService _todoService;

        public TodoCrudController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpGet("get-todo")]
        public async Task<IActionResult> GetTodoData()
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var todoDoc = await _todoService.GetTodoDocument(name);
            return Ok(todoDoc);
        }

        [HttpPost("add-category")]
        public async Task<IActionResult> SaveTodoCategory([FromBody] string todoCategory)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var process = await _todoService.AddNewTodoCategory(name, todoCategory);
            return Ok(process);
        }

        [HttpPost("add-task-to-category")]
        public async Task<IActionResult> SaveTaskToCategory([FromBody] TodoCategory taskToCategory)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var process = await _todoService.AddNewTaskToCategory(name, taskToCategory);
            return Ok(process);
        }

        [HttpPost("add-todo-list")]
        public async Task<IActionResult> SaveTaskToTodo([FromBody] ActiveTodoTask activeTodoTask)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var process = await _todoService.AddNewTaskTodoList(name, activeTodoTask);
            return Ok(process);
        }

        [HttpPost("remove-category")]
        public async Task<IActionResult> RemoveTodoCategory([FromBody] string categoryToRemove)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var process = await _todoService.RemoveTodoCategory(name, categoryToRemove);
            return Ok(process);
        }

        [HttpPost("remove-task")]
        public async Task<IActionResult> RemoveTodoTask([FromBody] TodoCategory todoCategory)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var process = await _todoService.RemoveTaskFromCategory(name, todoCategory);
            return Ok(process);
        }

        [HttpPost("update-todo-list")]
        public async Task<IActionResult> UpdateTodoList([FromBody] List<ActiveTodoTask> freshList)
        {
            var name = User.FindFirstValue(ClaimTypes.Name);
            var process = await _todoService.UpdateTodoList(name, freshList);
            return Ok(process);
        }
    }
}
