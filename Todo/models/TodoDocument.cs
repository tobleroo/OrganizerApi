using Newtonsoft.Json;

namespace OrganizerApi.Todo.models
{
    public class TodoDocument
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Owner { get; set; } = "not set";
        public List<TodoList> TodoLists { get; set; }
        public List<TaskCategory> TaskCategories { get; set; }
        //create constructor
        public TodoDocument()
        {
            TodoLists = new List<TodoList>();
            TaskCategories = new List<TaskCategory>();
        }
    }
}
