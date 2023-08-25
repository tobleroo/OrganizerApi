using OrganizerApi.Todo.models;

namespace OrganizerApi.Todo.Repository
{
    public interface ITodoRepository
    {

        void AddTaskCategory(string categoryName);

        void AddTaskToCategory(string categoryName, string taskName, int TimeToDo, int TimeToComplete, int TimesCompleted, string LastTimeCompleted);
        void DeleteTaskCategory(string categoryName);
        void DeleteTaskFromCategory(string categoryName, string taskName);

        Task<TodoDocument?> GetUserTodoData(string username);

        Task<TodoDocument> CreateTodoData(TodoDocument todoDb);

    }
}
