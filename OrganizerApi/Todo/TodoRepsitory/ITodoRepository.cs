using OrganizerBlazor.Todo.Models;

namespace OrganizerApi.Todo.TodoRepsitory
{
    public interface ITodoRepository
    {
        Task<TodoDocument> GetTodo(string username);
        Task<bool> UpsertTodo(string username, TodoDocument todoDoc);
    }
}
