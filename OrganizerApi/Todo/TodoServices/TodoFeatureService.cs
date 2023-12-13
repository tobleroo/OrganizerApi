using OrganizerApi.Diary.models.DiaryDTOs;
using OrganizerApi.Todo.Models.DTOs;
using OrganizerApi.Todo.TodoRepsitory;
using OrganizerApi.Todo.TodoUtils;

namespace OrganizerApi.Todo.TodoServices
{
    public class TodoFeatureService : ITodoFeatureService
    {

        private readonly ITodoRepository _todoRepository;

        public TodoFeatureService(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<ProcessDataGeneric<TodoSuggestionsDTO>> CreateTodoSuggestions(string username)
        {
            var processData = new ProcessDataGeneric<TodoSuggestionsDTO>();
            var todoDoc = await _todoRepository.GetTodo(username);
            var sugeestionsData = TodoSuggestionsFeatures.GetSuggestions(todoDoc.TodoCategories);

            processData.IsValid = true;
            processData.Message = "Here are the suggestions!";
            processData.Data = sugeestionsData;
            return processData;
        }
    }
}
