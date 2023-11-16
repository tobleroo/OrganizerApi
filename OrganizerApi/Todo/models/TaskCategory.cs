namespace OrganizerApi.Todo.models
{
    public class TaskCategory
    {
        public string categoryName { get; set; } = "not set";
        
        public List<RepeatableTask> Tasks { get; set; } = new List<RepeatableTask>();

        //create constructor
        public TaskCategory(string categoryName, List<RepeatableTask> Tasks)
        {
            this.categoryName = categoryName;
            this.Tasks = Tasks;
        }

        public TaskCategory()
        {
            
        }
    }
}
