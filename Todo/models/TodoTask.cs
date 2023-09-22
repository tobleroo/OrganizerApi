namespace OrganizerApi.Todo.models
{
    public class TodoTask
    {
        public string taskName { get; set; }
        public int TimeToDo { get; set; }
        //set isCompleted to false by default
        public bool isCompleted { get; set; } = false;
        

        //create constructor
        public TodoTask(string taskName, int TimeToDo)
        {
            this.taskName = taskName;
            this.TimeToDo = TimeToDo;
        }

        public TodoTask()
        {
            
        }
    }
}
