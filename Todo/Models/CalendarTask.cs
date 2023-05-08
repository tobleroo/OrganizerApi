namespace OrganizerApi.Todo.Models
{
    public class CalendarTask
    {
        private int Id { get; set; }
        private string TaskName { get; set; }
        private bool Completed { get; set; }

        private int TimeToComplete { get; set; }


        public CalendarTask(string taskName, bool completed, int timeToComplete)
        {
            TaskName = taskName;
            Completed = completed;
            TimeToComplete = timeToComplete;
            
        }
    }
}
