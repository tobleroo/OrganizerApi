using OrganizerApi.Todo.models;

namespace OrganizerApi.Todo.service
{
    public interface ITodoService
    {
        Task<TodoDocument> CreateTodoData(string username);
        Task<TodoDocument?> GetTodoData(string username);
        Task<bool> UpdateTodoData(TodoDocument todoDb);
    }
}
