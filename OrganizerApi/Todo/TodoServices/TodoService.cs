using OrganizerApi.Diary.models.DiaryDTOs;
using OrganizerApi.Todo.TodoRepsitory;
using OrganizerApi.Todo.TodoUtils;
using OrganizerBlazor.Todo.Models;

namespace OrganizerApi.Todo.TodoServices
{
    public class TodoService : ITodoService
    {

        private readonly ITodoRepository repository;

        public TodoService(ITodoRepository repository)
        {
            this.repository = repository;
        }

        public async Task<TodoDocument> GetTodoDocument(string username)
        {
            return await repository.GetTodo(username);
        }

        public async Task<ProcessData> AddNewTodoCategory(string username, string todoCat)
        {
            var process = new ProcessData() { IsValid = false, Message="Could not save category!"};

            var todoDoc = await repository.GetTodo(username);


            if(todoDoc.TodoCategories.Any(category => category.CategoryTitle == todoCat)){
                process.Message = "Category already exists!";
                return process;
            }

            todoDoc.TodoCategories.Add(new TodoCategory() { CategoryTitle = todoCat});

            var couldSave = await repository.UpsertTodo(todoDoc);
            
            if(couldSave) process.IsValid = true; process.Message = "Category saved!";

            return process;
        }

        //create a fake new category to get name of category wanted and the task wanted, todoactivity class works as DTO in this instance
        public async Task<ProcessData> AddNewTaskToCategory(string username, TodoCategory propCat)
        {
            var process = new ProcessData() { IsValid = false, Message = "Could not add activity!" };

            var todoDoc = await repository.GetTodo(username);

            var CategoryToAddTo = todoDoc.TodoCategories.Find(category => category.CategoryTitle ==  propCat.CategoryTitle);

            if (CategoryToAddTo == null) { 
                process.Message = "Cateorgy does not exists!"; 
                return process;
            }

            CategoryToAddTo.Activities.Add(propCat.Activities.First());

            var saveSuccess = await repository.UpsertTodo(todoDoc);
            if(saveSuccess) process.IsValid = true; process.Message = "Task Saved!";

            return process;
        }

        public async Task<ProcessData> AddNewTaskTodoList(string username, ActiveTodoTask activeTask)
        {
            var process = new ProcessData() { Message = "Could not save to list!" };

            var todoDoc = await repository.GetTodo(username);

            if(todoDoc.ActiveTodos.Contains(activeTask)) { 
                process.Message = "Activity already on list!";
                return process;
            };

            todoDoc.ActiveTodos.Add(activeTask);

            var couldSave = await repository.UpsertTodo(todoDoc);

            if(couldSave) process.IsValid = true; process.Message = "task added!";

            return process;
        }

        public async Task<ProcessData> RemoveTodoCategory(string username, string categoryToRemove)
        {
            var process = new ProcessData() { Message = "could not remove category!" };

            var todoDoc = await repository.GetTodo(username);
            todoDoc.TodoCategories = todoDoc.TodoCategories.Where(category => category.CategoryTitle != categoryToRemove).ToList();

            var success = await repository.UpsertTodo(todoDoc);
            if(success) process.IsValid = true; process.Message = "category removed!";

            return process;
        }

        public async Task<ProcessData> RemoveTaskFromCategory(string username, TodoCategory todoCategoryRemoveTask)
        {
            var process = new ProcessData() { Message = "could not remove task! try again" };

            var todoDoc = await repository.GetTodo(username);

            var todoCategory = todoDoc.TodoCategories.Where(cat => cat.CategoryTitle == todoCategoryRemoveTask.CategoryTitle).FirstOrDefault();

            if (todoCategory is null) return process;

            int removedCount = todoCategory.Activities.RemoveAll(activity => activity.Title == todoCategoryRemoveTask.Activities.First().Title);

            if (removedCount > 0)
            {
                process.Message = "Task removed!";
            }
            else
            {
                process.Message = "No task found to remove.";
            }

            var savingSuccess = await repository.UpsertTodo(todoDoc);

            if (savingSuccess)
            {
                process.IsValid = true;
                return process;
            }

            return process;
        }

        public async Task<ProcessDataGeneric<List<ActiveTodoTask>>> UpdateTodoList(string username, List<ActiveTodoTask> freshList)
        {
            var process = new ProcessDataGeneric<List<ActiveTodoTask>> { Message = "Could not update todo list!" };

            var todoDoc = await repository.GetTodo(username);

            todoDoc.TodoCategories = ActiveTodoListUtils.AddTodaysDateIfCompleted(freshList, todoDoc.TodoCategories);

            todoDoc.ActiveTodos = ActiveTodoListUtils.RemoveCompletedTasks(freshList);

            var successSave = await repository.UpsertTodo(todoDoc);

            if (successSave)
            {
                process.Message = "Successfully updated todolist!";
                process.IsValid = true;
                process.Data = todoDoc.ActiveTodos;
            }

            return process;
        }
    }
}
