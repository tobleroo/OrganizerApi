using OrganizerApi.Todo.models;
using OrganizerApi.Todo.Repository;

namespace OrganizerApi.Todo.service
{
    public class TodoService : ITodoService
    {

        private readonly ITodoRepository _todoContainerDb;

        public TodoService(ITodoRepository todoRepository) {
            _todoContainerDb = todoRepository;
        }

        public async Task<TodoDocument> CreateTodoData(string username)
        {
            //create a todo db for the user with the username as partion key
            var repeatTask = new RepeatableTask("Repeatable Task", 45, 20, "2023-23-32");
            var taskCat = new TaskCategory("Category", new List<RepeatableTask> { repeatTask });

            var todoList = new TodoList("List", new List<TodoTask> { new TodoTask("Task", 20) });

            var doc = new TodoDocument();
            doc.TaskCategories.Add(taskCat);
            doc.TodoLists.Add(todoList);

            doc.Owner = username;

            // and set a TodoContainerDb for the data
            var res = await _todoContainerDb.CreateTodoData(doc);
            return res;
        }

        public async Task<TodoDocument?> GetTodoData(string username)
        {
            //get the todo data for the user with the username as partion key
            var todoData = await _todoContainerDb.GetUserTodoData(username);
            if (todoData == null)
            {
                 await CreateTodoData(username);
                todoData = await _todoContainerDb.GetUserTodoData(username);
            }
            return todoData;
        }

        public async Task<bool> UpdateTodoData(TodoDocument todoDb)
        {
            var res = await _todoContainerDb.UpdateTodoData(todoDb);
            return res != null;
        }
    }
}
