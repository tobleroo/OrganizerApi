namespace OrganizerApi.Todo.models
{
    public class RepeatableTask
    {
        public string taskName { get; set; }
        public int TimeToComplete { get; set; }
        public int TimesCompleted { get; set; }
        public string LastTimeCompleted { get; set; }

        //create constructor
        public RepeatableTask(string taskName, int TimeToComplete, int TimesCompleted, string LastTimeCompleted)
        {
            this.taskName = taskName;
            this.TimeToComplete = TimeToComplete;
            this.TimesCompleted = TimesCompleted;
            this.LastTimeCompleted = LastTimeCompleted;
        }

        public RepeatableTask()
        {
        }
    }


}
