namespace OrganizerApi.Todo.models
{
    public class RepeatTask
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TimesDone { get; set; }
        public int TimeToDo { get; set; }
        public string LastestTimeDone { get; set; }

        public RepeatTask(string name, string description, int timesDone, int timeToDo, string lastestTimeDone)
        {
            Name = name;
            Description = description;
            TimesDone = timesDone;
            TimeToDo = timeToDo;
            LastestTimeDone = lastestTimeDone;
        }

    }
}