using OrganizerBlazor.Todo.Models;

namespace OrganizerApi.Todo.TodoRepsitory
{
    public interface ITodoRepository
    {
        Task<TodoDocument> GetTodo(string username);
        Task<bool> UpsertTodo(TodoDocument todoDoc);
    }
}
