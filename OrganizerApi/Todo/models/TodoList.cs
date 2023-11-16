namespace OrganizerApi.Todo.models
{
    public class TodoList
    {
        public string listName { get; set; } = "Name not set";
        public List<TodoTask> tasks { get; set; } = new List<TodoTask>();

        //create constructor
        public TodoList(string listName, List<TodoTask> tasks)
        {
            this.listName = listName;
            this.tasks = tasks;
        }
    }
}
