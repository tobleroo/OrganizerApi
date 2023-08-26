using OrganizerApi.Todo.models;

namespace OrganizerApi.Todo.Repository
{
    public interface ITodoRepository
    {

        Task<TodoDocument?> GetUserTodoData(string username);

        Task<TodoDocument> CreateTodoData(TodoDocument todoDb);

        Task<TodoDocument?> UpdateTodoData(TodoDocument todoDb);

    }
}
