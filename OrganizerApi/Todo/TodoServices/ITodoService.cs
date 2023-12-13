using OrganizerApi.Diary.models.DiaryDTOs;
using OrganizerBlazor.Todo.Models;

namespace OrganizerApi.Todo.TodoServices
{
    public interface ITodoService
    {

        Task<ProcessData> AddNewTaskToCategory(string username, TodoCategory propCat);
        Task<ProcessData> AddNewTaskTodoList(string username, ActiveTodoTask activeTask);
        Task<ProcessData> AddNewTodoCategory(string username, string todoCat);
        Task<TodoDocument> GetTodoDocument(string username);
        Task<ProcessData> RemoveTaskFromCategory(string username, TodoCategory todoCategoryRemoveTask);
        Task<ProcessData> RemoveTodoCategory(string username, string categoryToRemove);
        Task<ProcessDataGeneric<List<ActiveTodoTask>>> UpdateTodoList(string username, List<ActiveTodoTask> freshList);
    }
}
