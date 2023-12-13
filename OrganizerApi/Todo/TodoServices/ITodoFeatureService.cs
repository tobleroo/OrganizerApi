using OrganizerApi.Diary.models.DiaryDTOs;
using OrganizerApi.Todo.Models.DTOs;

namespace OrganizerApi.Todo.TodoServices
{
    public interface ITodoFeatureService
    {
        Task<ProcessDataGeneric<TodoSuggestionsDTO>> CreateTodoSuggestions(string username);
    }
}
