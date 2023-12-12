using OrganizerBlazor.Todo.Models;

namespace OrganizerApi.Todo.TodoUtils
{
    public class ActiveTodoListUtils
    {

        public static List<ActiveTodoTask> RemoveCompletedTasks(List<ActiveTodoTask> tasks)
        {
            var remainingTasks = tasks.Where(task => task.Done != true).ToList();
            return remainingTasks;
        }

        public static List<TodoCategory> AddTodaysDateIfCompleted(List<ActiveTodoTask> activeTasks, List<TodoCategory> allTasks) {

            // Flatten all Todos across all categories into a single list
            var allTodos = allTasks.SelectMany(category => category.Activities).ToList();

            foreach (var task in activeTasks)
            {
                if (!task.Done)
                {
                    // Find matching Todos by name and add today's date
                    allTodos.Where(todo => todo.Title == task.Name)
                            .ToList()
                            .ForEach(matchingTodo => matchingTodo.DatesWhenCompleted.Add(DateTime.Now.ToString("dd-MM-yyyy")));
                }
            }

            return allTasks;
        }
    }
}
